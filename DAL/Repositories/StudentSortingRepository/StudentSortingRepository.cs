using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using System.Linq.Dynamic.Core;

namespace DAL.Repository.StudentSortingRepository;

public class PagedEntityResults
{
    public List<Student> Items { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}

public class StudentSortingRepository : IStudentSortingRepository
{
    private readonly StudentSuccesContext _studentSuccesContext;

    public StudentSortingRepository(StudentSuccesContext studentSuccesContext)
    {
        _studentSuccesContext = studentSuccesContext;
    }
    public async Task<PagedEntityResults> GetSortingEntity(string term, string sort, int page, int limit)
    {

        IQueryable<Student> students;
        if (string.IsNullOrWhiteSpace(term))
            students = _studentSuccesContext.Students;
        else
        {
            term = term.Trim().ToLower();
            students = _studentSuccesContext
                .Students
                .Where(b => b.Address.ToLower().Contains(term)
                || b.FirstName.ToLower().Contains(term)
                || b.LastName.ToLower().Contains(term)
                || b.BirthPlace.ToLower().Contains(term)
                || b.Gender.ToLower().Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(sort))
        {
            var sortFields = sort.Split(','); 
            StringBuilder orderQueryBuilder = new StringBuilder();
            // using reflection to get properties of Student
            // propertyInfo= [FirstName,LastName,BirthPlace,Gender] 
            PropertyInfo[] propertyInfo = typeof(Student).GetProperties();


            foreach (var field in sortFields)
            {
               
                string sortOrder = "ascending";
              
                var sortField = field.Trim();
                if (sortField.StartsWith("-"))
                {
                    sortField = sortField.TrimStart('-');
                    sortOrder = "descending";
                }
              
                var property = propertyInfo.FirstOrDefault(a => a.Name.Equals(sortField, StringComparison.OrdinalIgnoreCase));
                if (property == null)
                    continue;
             
                orderQueryBuilder.Append($"{property.Name.ToString()} {sortOrder}, ");
            }
           
            string orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
            if (!string.IsNullOrWhiteSpace(orderQuery))
        
                students = students.OrderBy(orderQuery);
            else
                students = students.OrderBy(a => a.StudentId);
        }

        var totalCount = await _studentSuccesContext.Students.CountAsync();

        var totalPages = (int)Math.Ceiling(totalCount / (double)limit);


        var studentlist = await students.Skip((page - 1) * limit).Take(limit).ToListAsync();

        var studentdata = new PagedEntityResults
        {
            Items = studentlist,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
        return studentdata;
    }
}