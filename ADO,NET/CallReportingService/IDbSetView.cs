using ADONET.ReportingDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ADONET.CallReportingService;
public interface IDbSetView
{
    public Task <List<AverageScoreDTO>> GetGroupAverageScoresAsync();
    public Task<List<StudentDormitoryDTO>> StudentDormitoryNameAsync();
}
