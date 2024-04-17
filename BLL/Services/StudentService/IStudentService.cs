using BLL.StudentDto;
using DAL.StoredProcedureDTO;
namespace BLL.Services.StudentService;
public interface IStudentService
{
    Task<List<StudentDTO>> GetStudentAsync();
    Task AddStudentAsync(StudentDTO student);
    Task UpgradeStudentAsync(StudentDTO studentDTO);
    Task <StudentDTO>GetStudentByIdAsync( int studentid);
    Task <bool> DeleteStudentAsync(int id);
    Task CallCalculateScholarshipForAllStudentAsync(int month,int year);
    Task <StudentDTO> GetByIdAsync(int id);
    Task <IEnumerable<TopScoreResultDTO>>CallGetTopScoresProcedureAsync(int score);
    Task CallInsertStudentsDormitoryProcedureAsync();
    Task <IEnumerable<OverdueBookReportDTO>> CallOverdueBookReportAsync();
    Task  <IEnumerable<StudentRatingResult>>CallSortStudentRatingAsync();
    Task<string> ReturningBook(int studentId, int bookId, DateTime dateTime);
    Task<string> TakeBook(int studentId, int BookId);
}
