using DAL.BookDetailsDTO;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.BookDetails;
public class StudentBookDetailsRepository : IStudentBookDetailRepositorys
{
    private readonly StudentSuccesContext _context;
    public StudentBookDetailsRepository(StudentSuccesContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DalBookDetailsDto>> BookStudentDetails()
    {

        var query = from sb in _context.StudentBooks
                    join s in _context.Students on sb.StudentId equals s.StudentId
                    join b in _context.Books on sb.BookId equals b.BookId
                    select new DalBookDetailsDto
                    {
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Title = b.Title,
                        CheckStartDate = sb.CheckStartDate,
                        CheckEndDate = sb.CheckEndDate
                    };

        return await query.ToListAsync();
    }
}
