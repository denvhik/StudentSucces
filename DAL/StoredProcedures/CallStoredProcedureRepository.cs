﻿using Microsoft.EntityFrameworkCore;
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

    public async Task<List<ScholarshipDTO>> CallCalculateAcholarshipForStudentAsync(int studentID, int month, int year)
    {
        try
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("StudentConnections")))
            {
                var parametrs = new DynamicParameters();
                parametrs.Add("@StudentID", studentID);
                parametrs.Add("@Month", month);
                parametrs.Add("Year", year);

                var result = await connection.QueryAsync<ScholarshipDTO>("CalculateScholarshipForStudent", parametrs, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        catch (Exception ex)
        {
            throw SystemExeptionHandle.FromSystemException(ex);
        }
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

    public async  Task<string> CallReturnBookProcedureAsync(int studentId, int bookId, DateTime time)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("StudentConnections"));
        var parameters = new DynamicParameters();
        parameters.Add("@StudentID", studentId);
        parameters.Add("@BookID", bookId);
        parameters.Add("@CheckEndDate", time);
        parameters.Add("@ErrorMessage", dbType: DbType.String, direction: ParameterDirection.Output);

        try
        {
            await connection.ExecuteAsync("Sp_ReturnBook", parameters, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            throw SystemExeptionHandle.FromSystemException(ex);
        }
        var errorMessage = parameters.Get<string>("@ErrorMessage");

        return errorMessage;
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
    public async Task <string> CallTakeBookProcedureAsync(int studentId, int bookId)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("StudentConnections"));
        var parameters = new DynamicParameters();
        parameters.Add("@StudentID", studentId);
        parameters.Add("@BookID", bookId);
        //parameters.Add("@ErrorMessage", dbType: DbType.String, direction: ParameterDirection.Output);

        try
        {
            await connection.ExecuteAsync("Sp_TakeBook", parameters, commandType: CommandType.StoredProcedure);
        }
        catch(SqlException ex) 
        {
            throw new Exception(ex.Message);
        }
        catch (Exception ex)
        {
            throw SystemExeptionHandle.FromSystemException(ex);
        }
        var errorMessage = parameters.Get<string>("@ErrorMessage");
        
        return errorMessage;
    }
}
