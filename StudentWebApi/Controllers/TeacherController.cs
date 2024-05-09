using BLL.Services.TeacherService;
using BLL.StudentDto;
using Handling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StudentWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TeacherController : ControllerBase
{
    private readonly ITeacherService _teacherService;

    public TeacherController(ITeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    [HttpGet("GetTeachers")]
    [Authorize(Roles = "Admin,Member,Menager")]
    public async Task<ActionResult<List<TeachersDTO>>> GetTeacherList() 
    {
        List<TeachersDTO> teacherDTOs = new();
        try 
        {
            teacherDTOs = await _teacherService.GetTeacherAsync();
        }
        catch(Exception) 
        {
            throw new Exception();
        }
        return teacherDTOs;
    }
    [HttpDelete("DeleteTeacher/{id:int}")]
    [Authorize(Roles = "Admin,Menager")]
    public async Task<ActionResult<bool>> DeleteTeacher( int id ) 
    {
        if (id<=0)
        {
            throw new ArgumentException();
        }
        var student =await _teacherService.DeleteTeacherAsync(id);
        if (student == false)
        {
            throw new KeyNotFoundException("teacher with this id was not found");
        }
        return student;
    }
    [HttpPost("Create")]
    [Authorize(Roles = "Admin,Menager")]
    public async Task<ActionResult> CreateNewTeacher([FromBody]TeachersDTO teacherDTO) 
    {
         if (ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            await _teacherService.AddTeacherAsync(teacherDTO);

        }
        catch (Exception) 
        {
            throw new UserFriendlyException("somethinf went wrong");
        }
        return Ok("your created techer succsesfuly");
    }
    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> UpdatedTeacher([FromBody] TeachersDTO teacherDTO) 
    {
        if (ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            await _teacherService.UpgradeTeacherAsync(teacherDTO);
        }
        catch (ArgumentException) 
        {
            throw new ArgumentException();
        }
        catch (Exception) 
        {
            throw new SystemExeptionHandle("Something went wrong");
        }
        return Ok("Your Updated information about tacher succsesfully");
    }

}
