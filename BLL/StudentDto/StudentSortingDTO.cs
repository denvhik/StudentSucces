using System.ComponentModel.DataAnnotations;

namespace BLL.StudentDto;
public class StudentSortingDTO
{
    
    public List<StudentDTO> items { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}
