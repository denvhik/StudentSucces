using BLL.Services.StudentService;
using BLL.StudentDto;
using Handling;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace StudentWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly ILogger<StudentController> _logger;

    public StudentController(IStudentService studentService, ILogger<StudentController> logger)
    {
        _studentService = studentService;
        _logger = logger;
    }
    [HttpGet]
    public async Task<List<StudentDTO>> GetStudent()
    {
        return await _studentService.GetStudentAsync();

    }
    [HttpGet("GetByParametrStudent/{skip:int}/{take:int}")]
    public async Task<ActionResult<List<StudentDTO>>> GetByParametrStudent(int skip, int take)
    {
        if (skip <= 0 || take <= 0)
        {
            throw new ArgumentException();
        }
        try
        {
            await _studentService.GetStudentByParametrAsync(skip, take);
        } catch (Exception )
        {
            throw new Exception();
        }
        return Ok(new List<StudentDTO>());
    }
    [HttpPost("Create")]
    public async Task<IActionResult> CreateNewStudent([FromBody]StudentDTO studentDTO) 
    {
        if (!TryValidateModel(studentDTO)) 
        {
            throw new ValidationException();
        }
        await _studentService.AddStudentAsync(studentDTO);
        return Created();
    }
    [HttpPut("Update")]
    public async Task<IActionResult> UpdateStudent([FromBody] StudentDTO studentDTO)
    {
        try
        {
            await _studentService.UpgradeStudentAsync(studentDTO);

        }
        catch (ValidationException)
        {
            throw new ValidationException();
        }
        return Ok("StudentCreatedSuccesful");
    }
    [HttpDelete("DeleteStudent/{id:int}")]
    public async Task<ActionResult> DeleteStudent (int id) 
    {
        if (id <= 0) 
        {
            throw new ArgumentException();
        }
        var student =await _studentService.DeleteStudentAsync(id);
        if (!student)
        {
            throw new KeyNotFoundException();
        }
        return Ok($"the student with id:{id}deleted succesfuly");
    }

}
