using Microsoft.AspNetCore.Mvc;
using BLL.Services.StudentService;
using BLL.StudentDto;

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
    [HttpGet]
    public IActionResult GetStudentById(int studentId)
    {
        try
        {
            var student = _studentService.GetStudentByIdAsync(studentId);
            if (student != null)
            {
                return Json(new { success = true, student = student });
            }
            else
            {
                return Json(new { success = false, message = "Student not found" });
            }
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }


    [HttpPost]
    public async Task<IActionResult> Add([FromForm]StudentDTO studentDTO)
    {
        try
        {
            await _studentService.AddStudentAsync(studentDTO);
            return Ok(new { Message = $"Student Added successfully" });
        }
        catch (UserFriendlyException ex)
        {
            return Json(new { success=false, ErrorMessage = ex.Message });
        }
        catch (Exception ex)
        {
  
            var userFriendlyEx = ExceptionTranslator.Translate(ex);
            return StatusCode(500, new {succes= false, ErrorMessage = userFriendlyEx.Message });
        }
    }
    [HttpPost]
    public async Task<IActionResult> Delete(int studentId) 
    {
        try
        {
            var deleted = await _studentService.DeleteStudentAsync(studentId);
            if (deleted is true)
            {
                return Json(new { success = true, message = "Student delete successfully." });
            }
            else
            {
                throw new Exception("Student deletion failed.");
            }
        }
        catch (UserFriendlyException ex)
        {
            return Json(new { success = false, message = $"Failed to update student. Error: {ex.Message}" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"Failed to update student. Error: {ex.Message}" });
        }
    }
    [HttpPost]
    public async Task<IActionResult> UpdateStudentAsync([FromBody] StudentDTO studentDTO) 
    {
        try
        {
            await _studentService.UpgradeStudentAsync( studentDTO);
            return new JsonResult(new { Message = "Student updated successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = $"Failed to update student. Error: {ex.Message}" });
        }
    }

}
