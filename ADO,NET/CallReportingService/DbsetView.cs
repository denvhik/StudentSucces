using ADONET.ReportingDTO;
using ADONET.SimpleOperationsService;
using System.Collections.Generic;
using System;
using System.Data;
using System.Threading.Tasks;
namespace ADONET.CallReportingService;
public class DbsetView : IDbSetView
{
    private readonly IReportingService _reportingService;
    public DbsetView(IReportingService reportingService) 
    {
        _reportingService = reportingService;
    }
    public async Task<List<AverageScoreDTO>> GetGroupAverageScoresAsync()
    {
        DataTable dataTable = await _reportingService.GetGroupAverageScoresAsync();
        List<AverageScoreDTO> averageScores = new List<AverageScoreDTO>();

        foreach (DataRow dr in dataTable.Rows)
        {
            AverageScoreDTO averageScoreDTO = new ()
            {
                GroupName = dr["GroupName"].ToString(), 
                AverageScore = Convert.ToInt32(dr["AverageScore"]) 
            };

            averageScores.Add(averageScoreDTO);
        }
        return averageScores;
    }

    public async  Task<List<StudentDormitoryDTO>> StudentDormitoryNameAsync()
    {
        DataTable dataTable = await _reportingService.GetStudentsInDormitoriesAsync();
        List<StudentDormitoryDTO> studentDormitoryName= new();
        foreach (DataRow dr in dataTable.Rows)
        {
            StudentDormitoryDTO studentDormitoryNameDTO = new()
            {
                FirstName = dr["FirstName"].ToString(),
                LastName = dr["LastName"].ToString(),
                DormitoryName = dr["DormitoryName"].ToString()
            };
            studentDormitoryName.Add(studentDormitoryNameDTO);
        }
        return studentDormitoryName;
    }
}
