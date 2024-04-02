﻿using BLL.StudentDto;
using DAL.StoredProcedureDTO;


namespace BLL.Services.StudentService;

public interface IStudentService
{
    Task<IEnumerable<StudentDTO>> GetStudentAsync();
    Task AddStudentAsync(StudentDTO student);
    Task UpgradeStudentAsync(int id,StudentDTO studentDTO);
    Task <bool> DeleteStudentAsync(int id);
    Task CallCalculateScholarshipForAllStudentAsync(int month,int year);
    Task <StudentDTO> GetByIdAsync(int id);
    Task <IEnumerable<TopScoreResultDTO>>CallGetTopScoresProcedureAsync(int score);
    Task CallInsertStudentsDormitoryProcedureAsync();
    Task <IEnumerable<OverdueBookReportDTO>> CallOverdueBookReportAsync();
    Task  <IEnumerable<StudentRatingResult>>CallSortStudentRatingAsync();

}