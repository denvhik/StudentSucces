using BLL.StudentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.TeacherService;

public interface ITeacherService
{
    Task<List<TeacherDTO>> GetTeacherAsync();
    Task AddTeacherAsync(TeacherDTO hobbie);
    Task UpgradeTeacherAsync(TeacherDTO teacherDTO);
    Task<bool> DeleteTeacherAsync(int id);
}
