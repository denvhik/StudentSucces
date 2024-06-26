﻿using Microsoft.EntityFrameworkCore;
using DAL.StoredProcedureDTO;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Handling;
namespace DAL.StoredProcedures;
/// <summary>
/// 
/// </summary>
public class CallStoredProcedureRepository : ICallStoredProcedureRepository
{
    /// <summary>
    /// 
    /// </summary>
    private readonly IConfiguration _configuration;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    /// 
    public CallStoredProcedureRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="studentID"></param>
    /// <param name="month"></param>
    /// <param name="year"></param>
    /// <returns></returns>
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

    public async Task<string> CallInsertStudentsDormitoryProcedureAsync(List<int>studentId,int? dormitoryId)
    {
        try
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("StudentConnections")))
            {
                var studentTable = new DataTable();
                studentTable.Columns.Add("StudentIDs", typeof(int));
                foreach (var id in studentId)
                {
                    studentTable.Rows.Add(id);
                }
                var parameters = new DynamicParameters();
                parameters.Add("@StudentIDs", studentTable.AsTableValuedParameter("studentidtabletype"));
                parameters.Add("@DormitoryID", dormitoryId);

              
                var result = await connection.QueryAsync<string>("SP_InsertStudentsDormitories", parameters, commandType: CommandType.StoredProcedure);

                return result.FirstOrDefault();
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
        parameters.Add("@ReturnDate", time);
        try
        {
            await connection.ExecuteAsync("Sp_ReturnBook", parameters, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            throw SystemExeptionHandle.FromSystemException(ex);
        }

        string message = "Success";
        return message;
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
        try
        {
            await connection.ExecuteAsync("Sp_TakeBook", parameters, commandType: CommandType.StoredProcedure);
        }
        catch(SqlException ex) 
        {
            throw SystemExeptionHandle.FromSystemException(ex);
        }
        catch (Exception ex)
        {
            throw SystemExeptionHandle.FromSystemException(ex);
        }
        string message = "Success";
        return message;
    }
}
