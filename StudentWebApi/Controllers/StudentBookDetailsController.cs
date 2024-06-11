using BLL.Services.StudentBookService;
using BLL.StudentDto;
using Microsoft.AspNetCore.Authorization;
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
    /// <summary>
    /// Retrieves a list of books checked out by students, including student names and book titles.
    /// </summary>
    /// <returns>A list of book details associated with students, including names, book titles, and check-out dates.</returns>
    /// <exception cref="Exception">Thrown when there is an error retrieving the book details from the database.</exception>
    [HttpGet]
    [AllowAnonymous]
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
