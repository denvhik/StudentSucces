using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;


namespace ADONET.SimpleOperationsService;

public class ReportingService : IReportingService
{
    private readonly IConfiguration _configuration;
    public ReportingService(IConfiguration configuration) 
    {
        _configuration = configuration;
    }

    public async Task<DataTable> GetGroupAverageScoresAsync()
    {
        DataTable dataTable = new ();
        string query = @"
            SELECT 
                 g.[GroupName], 
                 AVG([ss].[Score]) AS AverageScore
            FROM
                [dbo].[StudentGroup] [sg]
            INNER JOIN
                 [dbo].[StudentSubject] [ss] ON [sg].[StudentID] = [ss].[StudentID]
            INNER JOIN 
                 [dbo].[Groups] [g] ON [sg].[GroupID] = [g].[GroupID] 
            GROUP BY
                  g.[GroupName] ";

        using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:StudentConnections"]))
        {
            await connection.OpenAsync();
            SqlCommand command = new (query, connection);
            SqlDataAdapter adapter = new (command);
            await Task.Run(() => adapter.Fill(dataTable));
        }
        return dataTable;
    }


    public  async Task<DataTable> GetStudentsInDormitoriesAsync()
    {
        DataTable dataTable = new();
        string query  = @"
                SELECT 
                    [s].FirstName,
                    [s].LastName,
                    [d].DormitoryName
                FROM (
                    SELECT [s].*, [sd].[DormitoryID]
                FROM 
                    [dbo].[Student] [s]
                INNER JOIN
                    [dbo].[StudentsDormitory] [sd] ON [s].[StudentID] = [sd].[StudentID]
                ) AS [s]
                INNER JOIN 
                    [dbo].[Dormitory] [d] ON [s].[DormitoryID] = [d].[DormitoryID]";
        using (SqlConnection connection = new(_configuration["ConnectionStrings:StudentConnections"]))
        {
           await connection.OpenAsync();
            SqlCommand sqlCommand = new (query, connection);
            SqlDataAdapter adapter = new (sqlCommand);
            await Task.Run(()=> adapter.Fill(dataTable));
        }
        return dataTable;
    }
}
