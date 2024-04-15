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
        List<StudentDTO> students = await _studentService.GetStudentAsync();
        //const int pageSize = 5;
        //if (pg < 1) pg = 1;
        //int recsCount = students.Count();
        //Pager pager = new Pager(recsCount, pg, pageSize);
        //int reskip = (pg - 1) * pageSize;
        //var data = students.Skip(reskip).Take(pager.PageSize).ToList();
        //ViewBag.Pager = pager;
        return View(students);
    }
    [HttpGet]
    public IActionResult GetStudentById(int Id)
    {
        try
        {
            var student = _studentService.GetStudentByIdAsync(Id);
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
    [ValidateAntiForgeryToken]
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int Id) 
    {
        try
        {
            var deleted = await _studentService.DeleteStudentAsync(Id);
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStudent(StudentDTO studentDTO) 
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
    [HttpGet]
    public async  Task<IActionResult>GetStudents()
    {
        List<StudentDTO> students = await _studentService.GetStudentAsync();
        return Json(students);
    }
}
