using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace AdoNet.ViewService;
public class CallViewService:ICallViewService
{
    private readonly IConfiguration _configuration;
    public CallViewService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public  async Task ReadDataFromView()
    {
        try
        {
            using var connection = new SqlConnection(_configuration["ConnectionStrings:StudentConnections"]);
            await connection.OpenAsync();
            string query = "SELECT * FROM VW_StudentGroupView";
            SqlCommand command = new(query, connection);
            SqlDataReader reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                int studentId = reader.GetInt32(0);
                string firstName = reader.GetString(1);
                string lastName = reader.GetString(2);
                int groupId = reader.GetInt32(3);
                string groupName = reader.GetString(4);
            }
        }
        catch (SqlException ex)
        {
            return;
        }
        
    }
}