using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;



namespace ADONET.SimpleOperationsService;

public class Reporting : IReporting
{
    private readonly IConfiguration _configuration;
    public Reporting(IConfiguration configuration) 
    {
        _configuration = configuration;
    }

    public async Task<DataTable> GetGroupAverageScores()
    {
        DataTable dataTable = new ();
        string query = @"
            SELECT
                [sg].[GroupID],
                AVG([ss].[Score]) AS AverageScore
            FROM
                [dbo].[StudentGroup] [sg]
            INNER JOIN [dbo].[StudentSubject] [ss] ON [sg].[StudentID] = [ss].[StudentID]
            GROUP BY
                [sg].[GroupID]";

        using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:StudentConnections"]))
        {
           await connection.OpenAsync();
            SqlCommand command = new (query, connection);
            SqlDataAdapter adapter = new (command);
            await Task.Run(() => adapter.Fill(dataTable));
        }
        return dataTable;
    }

    public  async Task<DataTable> GetStudentsInDormitories()
    {
        DataTable dataTable = new();
        string query  = @"
            SELECT [s].*
            FROM (
                SELECT [s].*
                FROM
                    [dbo].[Student] [s]
                INNER JOIN 
                    [dbo].[StudentsDormitory] [sd] ON [s].[StudentID] = [sd].[StudentID]
                WHERE 
                    [sd].[DormitoryID] BETWEEN 2 AND 4
            ) AS [s]";
        using (SqlConnection connection = new(_configuration["ConnectionStrings:StudentConnections"]))
        {
           await connection.OpenAsync();
            SqlCommand sqlCommand = new (query, connection);
            SqlDataAdapter adapter = new (sqlCommand);
            await Task.Run(()=> adapter.Fill(dataTable));
        }
        return dataTable ;
    }
}
