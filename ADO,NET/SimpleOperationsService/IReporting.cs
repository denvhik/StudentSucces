using System.Data;
using System.Threading.Tasks;
namespace ADONET.SimpleOperationsService;

public interface IReporting
{
    public Task <DataTable> GetGroupAverageScores();
    public Task<DataTable> GetStudentsInDormitories();
}
