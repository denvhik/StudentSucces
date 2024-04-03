
using ADONET.ReportingDTO;
using System.Threading.Tasks;

namespace ADONET.CallReportingService;
public interface IDbSetView
{
    public Task<AverageScoreDTO> GetGroupAverageScoresAsync();
    public Task<StudentDormitoryDTO> StudentDormitoryNameAsync();
}
