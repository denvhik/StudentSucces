using Microsoft.EntityFrameworkCore;
using DAL.StoredProcedureDTO;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using DAL.SystemExeptionHandling;

namespace DAL.StoredProcedures;

public class CallStoredProcedureRepository : ICallStoredProcedureRepository
{
    private readonly IConfiguration _configuration;

    public CallStoredProcedureRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<List<ScholarshipDTO>> CallCalculateScholarshipForAllStudentAsync(int month, int year)
    {
        try
        {
            using (var connection = new SqlConnection (_configuration.GetConnectionString("StudentConnections"))) 
            {
                var parametrs = new DynamicParameters();
                parametrs.Add("@Month",month);
                parametrs.Add("Year", year);

                var result = await  connection.QueryAsync<ScholarshipDTO>("CalculateScholarshipForAllStudents", parametrs, commandType: CommandType.StoredProcedure);
                 return result.ToList();
            }
        }
        catch (Exception ex)
        {
           throw SystemExeptionHandle.FromSystemException(ex);
        }
    }

    public async Task<List<TopScoreResultDTO>> CallGetTopScoresProcedureAsync(int score)
    {

        try
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("StudentConnections")))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@TopCount", score);

                var result = await connection.QueryAsync<TopScoreResultDTO>("SP_GetTopScores", parameters, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }
        catch (Exception ex)
        {
            throw SystemExeptionHandle.FromSystemException(ex);
        }
    }

    public async Task<bool> CallInsertStudentsDormitoryProcedureAsync()
    {
        try
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("StudentConnections")))
            {
               var result = await  connection.ExecuteAsync("SP_InsertStudentsDormitory", commandType: CommandType.StoredProcedure);
                return true; 
            }
        }
        catch (Exception ex)
        {
            throw SystemExeptionHandle.FromSystemException(ex);
        }
    }

    public async Task<List<OverdueBookReportDTO>> CallOverdueBookReportAsync()
    {

        try
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("StudentConnections")))
            {

                var result = await connection.QueryAsync<OverdueBookReportDTO>("SP_GetOverdueBooksReport", commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }
        catch (Exception ex)
        {
            throw SystemExeptionHandle.FromSystemException(ex);
        }
    }

    public async Task<List<StudentRatingResult>> CallSortStudentRatingAsync()
    {
        try 
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("StudentConnections")))
            {
                var result = await connection.QueryAsync<StudentRatingResult>("SP_SortStudentRating", commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        catch (Exception ex)
        {
            throw SystemExeptionHandle.FromSystemException(ex);
        }

    }

}
