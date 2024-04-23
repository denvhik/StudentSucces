using BLL.Services.StudentService;
using BLL.StudentDto;
using DAL.Models;
using Handling;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

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
        List<StudentDTO> studentDTOs = new List<StudentDTO>();

        studentDTOs= await _studentService.GetStudentAsync();
        if (studentDTOs != null) 
        {
            throw new KeyNotFoundException();
        }
        return studentDTOs;
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
    public async Task<ActionResult> CreateNewStudent([FromBody]StudentDTO studentDTO) 
    {
        if (!TryValidateModel(studentDTO)) 
        {
            throw new ValidationException();
        }
        try 
        { 
            await _studentService.AddStudentAsync(studentDTO);
        }
        catch (Exception) 
        {
            throw new UserFriendlyException("something went wrong");
        }

        return Created();
    }
    [HttpPut("Update")]
    public async Task<ActionResult> UpdateStudent([FromBody] StudentDTO studentDTO)
    {
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
    [HttpPost("BookMenagment")]
    public async Task<ActionResult<string>> BookMenagment(int studentId, int bookId, [Optional] DateTime? time) 
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
        catch (UserFriendlyException ex)
        {
            throw new UserFriendlyException(ex);
        }
        catch (Exception ) 
        {
            throw new  Exception();
        }
    }
    [HttpPost("InsertStudentToDormitory")]
    public async Task<ActionResult> AddSrudentToDormotory(List<int> studentId,[Optional]int DormitoryId) 
    {
        var result = await _studentService.CallInsertStudentsDormitoryProcedureAsync(studentId,DormitoryId);
        return Ok(result);
    }
}
