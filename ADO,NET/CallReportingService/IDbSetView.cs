using AdoNet.ReportingDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdoNet.CallReportingService;
public interface IDbSetView
{
    public Task <List<AverageScoreDTO>> GetGroupAverageScoresAsync();
    public Task<List<StudentDormitoryDTO>> StudentDormitoryNameAsync();
}
