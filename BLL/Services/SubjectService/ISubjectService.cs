using BLL.StudentDto;
namespace BLL.Services.SubjectService;

public interface ISubjectService
{
    Task<IEnumerable<SubjectDTO>> GetHobbieAsync();
    Task AddHobbieAsync(SubjectDTO subject);
    Task UpgradeHobbieAsync(int id, SubjectDTO subjectDTO);
    Task<bool> DeleteHobbieAsync(int id);
}
