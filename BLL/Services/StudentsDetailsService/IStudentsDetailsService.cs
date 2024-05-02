using BLL.StudentDto;

namespace BLL.Services.StudentsDetailsService;
public interface IStudentsDetailsService
{
    Task<StudentsJoinedEntetiesDTO> GetStudentEntetyByIdAsync(int id, params string[] includes);
}
