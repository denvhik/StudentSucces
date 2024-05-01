using Asp.Versioning;
using AutoMapper;
using BLL.Services.StudentsDetailsService;
using BLL.Services.StudentService;
using BLL.StudentDto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using Sieve.Services;
using StudentWebApi.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace StudentWebApi.Controllers;
[ApiVersion("1.0")]
[Route("api/[controller]")]
[ApiController]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly ILogger<StudentController> _logger;
    private readonly SieveProcessor _sieveProcessor;
    private readonly IMapper _mapper;
    private readonly IStudentsDetailsService _studentsDetailsService;

    public StudentController(IStudentService studentService, ILogger<StudentController> logger, SieveProcessor sieveProcessor, IMapper mapper, IStudentsDetailsService studentsDetailsService)
    {
        _studentService = studentService;
        _logger = logger;
        _sieveProcessor = sieveProcessor;
        _mapper = mapper;
        _studentsDetailsService = studentsDetailsService;
    }

    [HttpGet("GetStudent")]
    public async Task<IActionResult> GetStudent([FromQuery] SieveModel model)
    {
        List<StudentDTO> studentDTOs = new List<StudentDTO>();

        studentDTOs = await _studentService.GetStudentAsync();
        var studentresult = _mapper.Map<List<StudentApiDto>>(studentDTOs);
        IQueryable<StudentApiDto> queryableStudents = studentresult.AsQueryable();
        queryableStudents = _sieveProcessor.Apply(model, queryableStudents);
        if (queryableStudents == null)
        {
            throw new KeyNotFoundException();
        }
        return Ok(queryableStudents);
    }
    [HttpGet("StudentSortingFiltering")]
    public async Task<IActionResult> GetBooks(string term, string sort, int page = 1, int limit = 10)
    {
        var results = await _studentService.GetSortingEntity(term, sort, page, limit);

        // AddAsync pagination headers to the response
        Response.Headers.Append("X-Total-Count", results.TotalCount.ToString());
        Response.Headers.Append("X-Total-Pages", results.TotalPages.ToString());
        return Ok(results);
    }
    [HttpGet("GetByParametrStudent")]
    public async Task<ActionResult<List<StudentDTO>>> GetByParametrStudent([FromQuery] int skip, int take)
    {

        List<StudentDTO> studentDTOs = new List<StudentDTO>();

        if (skip <= 0 || take <= 0)
        {
            throw new ArgumentException();
        }
        try
        {
            studentDTOs = await _studentService.GetStudentByParametrAsync(skip, take);
        }
        catch (Exception)
        {
            throw new CustomException("Oops something went wrong");
        }
        return Ok(studentDTOs);
    }
    
    [HttpPost("Create")]
    public async Task<ActionResult> CreateNewStudent([FromBody] StudentDTO studentDTO)
    {
        if (!TryValidateModel(studentDTO))
            throw new ValidationException();
        try
        {
            await _studentService.AddStudentAsync(studentDTO);
        }
        catch (Exception)
        {
            throw new CustomException("Oops something went wrong");
        }
        return Ok();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="studentDTO"></param>
    /// <returns></returns>
    /// <exception cref="ValidationException"></exception>
   
    [HttpPut("Update")]
    public async Task<ActionResult> UpdateStudent([FromBody] StudentDTO studentDTO)
    {

        if (!TryValidateModel(studentDTO))
            throw new ValidationException();
        try
        {
            await _studentService.UpgradeStudentAsync(studentDTO);

        }
        catch (ValidationException)
        {
            throw new ValidationException();
        }
        return Ok("Student updated Succesful");
    }
    [HttpDelete("DeleteStudent")]
    public async Task<ActionResult> DeleteStudent([FromQuery] int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException();
        }
        var student = await _studentService.DeleteStudentAsync(id);
        if (!student)
        {
            throw new KeyNotFoundException();
        }
        return Ok($"the student with id:{id}deleted succesfuly");
    }
    [HttpPost("BookMenagment")]
    public async Task<ActionResult<string>> BookMenagment([FromQuery] int studentId, int bookId, [Optional] DateTime? time)
    {
        try
        {
            if (time is null)
            {
                var takebook = await _studentService.TakeBook(studentId, bookId);
                return Ok("succesfuly");
            }
            var returnbook = await _studentService.ReturningBook(studentId, bookId, (DateTime)time);
            return Ok("succesfuly returning book");
        }
        catch (UserFriendlyException)
        {
            throw new CustomException("Oops something went wrong");
        }
        catch (Exception)
        {
            throw new Exception();
        }
    }
    [HttpPost("InsertStudentToDormitory")]
    public async Task<ActionResult<string>> AddSrudentToDormotory([FromBody] List<int> studentId, int? dormitoryId)
    {
        var result = await _studentService.CallInsertStudentsDormitoryProcedureAsync(studentId, dormitoryId);
        return Ok(result);
    }
    [HttpPatch("UpdatePartial/{studentId}")]
    public async Task<ActionResult> UpdateStudent(int studentId, [FromBody] JsonPatchDocument<StudentDTO> patchdocument)
    {
        if (patchdocument == null)
        {
            
            return BadRequest("Invalid patch document");
        }
        try
        {
            var studentDTO = await _studentService.GetStudentByIdAsync(studentId);
            if (studentDTO == null)
            {
                return NotFound($"Student with ID {studentId} not found");
            }

            patchdocument.ApplyTo(studentDTO, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _studentService.UpgradeStudentAsync(studentDTO);
        }
        catch (ValidationException)
        {
            throw new ValidationException();
        }
        return Ok("Student updated successfully");
    }
    [HttpGet("GetStudentInfo/{id:int}")]
    public async Task<StudentsJoinedEntetiesDTO> GetStudentInfo(int id,[FromQuery] params string[] includes) 
    {
        try 
        {
            var result = await _studentsDetailsService.GetStudentEntetyByIdAsync(id,includes);
            return result;
        }
        catch (Exception)
        {
            throw new KeyNotFoundException();
        }
    }
}