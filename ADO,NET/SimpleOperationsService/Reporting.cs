using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using Microsoft.Data.SqlClient;



namespace ADONET.SimpleOperationsService;

public class Reporting : IReporting
{
    private readonly IConfiguration _configuration;
    public Reporting(IConfiguration configuration) 
    {
        _configuration = configuration;
    }

    public DataTable GetGroupAverageScores()
    {
        DataTable dataTable = new DataTable();
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
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
             adapter.Fill(dataTable);
        }
        return dataTable;
    }

    public DataTable GetStudentsInDormitories()
    {
        throw new NotImplementedException();
    }
}
