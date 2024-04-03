using ADONET.ReportingDTO;
using ADONET.SimpleOperationsService;
using System;
using System.Threading.Tasks;
namespace ADONET.CallReportingService;
public class DbsetView : IDbSetView
{
    private readonly IReportingService _reportingService;
    public DbsetView(IReportingService reportingService) 
    {
        _reportingService = reportingService;
    }
    public async Task<AverageScoreDTO> GetGroupAverageScoresAsync()
    {
        throw new NotImplementedException();
    }

    public async  Task<StudentDormitoryDTO> StudentDormitoryNameAsync()
    {
        throw new NotImplementedException();
    }
}
