using Microsoft.Extensions.Configuration;
using System;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using ADONET.ViewService;
namespace ADO_NET.ViewService;
public class CallView:ICallView
{
    private readonly IConfiguration _configuration;
    public CallView(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public  async Task ReadDataFromView()
    {
        try
        {
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:StudentConnections"]))
            {
                await connection.OpenAsync();

                string query = "SELECT * FROM VW_StudentGroupView";
                SqlCommand command = new(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                Console.WriteLine("-------------------------------------------------------------");
                Console.WriteLine("| StudentID | FirstName | LastName | GroupID | GroupName   |");
                Console.WriteLine("-------------------------------------------------------------");
                while (await reader.ReadAsync())
                {
                    int studentId = reader.GetInt32(0);
                    string firstName = reader.GetString(1);
                    string lastName = reader.GetString(2);
                    int groupId = reader.GetInt32(3);
                    string groupName = reader.GetString(4);

                    Console.WriteLine($"| {studentId,-9} | {firstName,-9} | {lastName,-8} | {groupId,-7} | {groupName,-12} |");
                }
                Console.WriteLine("-------------------------------------------------------------");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
        
    }
}