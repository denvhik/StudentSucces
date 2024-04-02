using System;
using System.Data;
namespace ADONET.SimpleOperationsService;

public interface IReporting
{
    DataTable GetGroupAverageScores();
    DataTable GetStudentsInDormitories();
}
