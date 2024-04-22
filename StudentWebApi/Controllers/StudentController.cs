using BLL.Services.StudentService;
using BLL.StudentDto;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
    public async Task<ActionResult<List<StudentDTO>>> GetStudent()
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
        return await _studentService.GetStudentByParametrAsync(skip, take);

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
    [Htt]
}
