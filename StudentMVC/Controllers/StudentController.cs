using Microsoft.AspNetCore.Mvc;
using BLL.Services.StudentService;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace StudentMVC.Controllers;
public class StudentController : Controller
{
    private readonly IStudentService _studentService;
    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await _studentService.GetStudentAsync();
        return View(result);
    }
    [HttpPost]
    public async Task<IActionResult> AddStudentAsync([FromBody]BLL.StudentDto.StudentDTO studentDTO)
    {
        try
        {
            await _studentService.AddStudentAsync(studentDTO);
            return new JsonResult(new { Message = "Student added successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = $"Failed to add student. Error: {ex.Message}" });
        }
    }
    [HttpDelete]
    public async Task<IActionResult> DeleteStudentAsync([FromBody] int id) 
    {
        try
        {
            await _studentService.DeleteStudentAsync(id);
            return new JsonResult(new { Message = "Student deleted successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = $"Failed to delete student. Error: {ex.Message}" });
        }
    }
    [HttpPost]
    public async Task<IActionResult> UpdateStudentAsync([FromBody] int id, BLL.StudentDto.StudentDTO studentDTO) 
    {
        try
        {
            await _studentService.UpgradeStudentAsync(id, studentDTO);
            return new JsonResult(new { Message = "Student updated successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = $"Failed to update student. Error: {ex.Message}" });
        }
    }

}
