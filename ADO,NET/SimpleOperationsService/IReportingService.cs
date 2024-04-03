using System.Data;
using System.Threading.Tasks;
namespace ADONET.SimpleOperationsService;

public interface IReportingService
{
    public Task <DataTable> GetGroupAverageScoresAsync();
    public Task<DataTable> GetStudentsInDormitoriesAsync();
}
