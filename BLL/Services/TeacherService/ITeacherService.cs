using BLL.StudentDto;

namespace BLL.Services.TeacherService;

public interface ITeacherService
{
    Task<List<TeachersDTO>> GetTeacherAsync();
    Task AddTeacherAsync(TeachersDTO hobbie);
    Task UpgradeTeacherAsync(TeachersDTO teacherDTO);
    Task<bool> DeleteTeacherAsync(int id);
}
