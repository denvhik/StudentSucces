using BLL.StudentDto;
namespace BLL.Services.SubjectService;

public interface ISubjectService
{
    Task<IEnumerable<SubjectDTO>> GetSubjectAsync();
    Task AddSubjectAsync(SubjectDTO subject);
    Task UpgradeSubjectAsync(int id, SubjectDTO subjectDTO);
    Task<bool> DeleteSubjectAsync(int id);
}
