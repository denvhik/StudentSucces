﻿using DAL.StoredProcedureDTO;
namespace DAL.StoredProcedures;
public interface ICallStoredProcedureRepository
{
    Task<List<ScholarshipDTO>> CallCalculateScholarshipForAllStudentAsync(int month, int year);
    Task<List<ScholarshipDTO>> CallCalculateAcholarshipForStudentAsync(int studentID,int month, int year);
    Task<List<TopScoreResultDTO>> CallGetTopScoresProcedureAsync(int score);
    Task <string> CallInsertStudentsDormitoryProcedureAsync(List<int>studentId,int? dormitoryId);
    Task<List<StudentRatingResult>> CallSortStudentRatingAsync();
    Task<List<OverdueBookReportDTO>> CallOverdueBookReportAsync();
    Task <string>  CallTakeBookProcedureAsync(int studentId, int bookId);
    Task <string>  CallReturnBookProcedureAsync(int studentId, int bookId,DateTime time);
}