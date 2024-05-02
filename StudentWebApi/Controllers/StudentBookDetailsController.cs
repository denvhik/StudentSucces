using BLL.Services.StudentBookService;
using BLL.Services.StudentService;
using BLL.StudentDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StudentWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentBookDetailsController : ControllerBase
{
    private readonly IStudentBookDetails _studentBookDetailsService;
    private readonly ILogger<StudentBookDetailsController> _logger;

    public StudentBookDetailsController(IStudentBookDetails studentBookDetailsService, ILogger<StudentBookDetailsController> logger)
    {
        _studentBookDetailsService = studentBookDetailsService;
        _logger = logger;
    }
    [HttpGet]
    public async Task<ActionResult<List<BllStudentBookDTO>>> GetStudentBookDetails() 
    {
        List<BllStudentBookDTO> studentBookDTOs = new List<BllStudentBookDTO>();
        try
        {
             studentBookDTOs = await _studentBookDetailsService.StudentBookDetails();
        } catch (Exception ) 
        {
            throw new Exception();
        }
        return Ok(studentBookDTOs);
    }
}
