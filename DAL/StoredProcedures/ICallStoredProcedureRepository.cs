using DAL.StoredProcedureDTO;
namespace DAL.StoredProcedures
{
    public interface ICallStoredProcedureRepository
    {
        Task<List<ScholarshipDTO>> CallCalculateScholarshipForAllStudentAsync(int month, int year);
        Task<List<ScholarshipDTO>> CallCalculateAcholarshipForStudentAsync(int studentID,int month, int year);
        Task<List<TopScoreResultDTO>> CallGetTopScoresProcedureAsync(int score);
        Task <bool> CallInsertStudentsDormitoryProcedureAsync();
        Task<List<StudentRatingResult>> CallSortStudentRatingAsync();
        Task<List<OverdueBookReportDTO>> CallOverdueBookReportAsync();
    }
}
