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
    /// <summary>
    /// Retrieves a list of teachers.
    /// </summary>
    /// <returns>A list of <see cref="TeachersDTO"/> objects.</returns>
    /// <exception cref="Exception">Thrown when an error occurs while fetching the teacher list.</exception>
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
    /// <summary>
    /// Deletes a teacher by their ID.
    /// </summary>
    /// <param name="id">The ID of the teacher to delete.</param>
    /// <returns>A boolean indicating whether the teacher was successfully deleted.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided ID is invalid (<= 0).</exception>
    /// <exception cref="KeyNotFoundException">Thrown when a teacher with the specified ID is not found.</exception>
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
    /// <summary>
    /// Creates a new teacher.
    /// </summary>
    /// <param name="teacherDTO">The <see cref="TeachersDTO"/> object representing the new teacher.</param>
    /// <returns>An action result indicating the outcome of the operation.</returns>
    /// <exception cref="UserFriendlyException">Thrown when an error occurs during the creation process.</exception>
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
    /// <summary>
    /// Updates an existing teacher's information.
    /// </summary>
    /// <param name="teacherDTO">The <see cref="TeachersDTO"/> object containing the updated teacher information.</param>
    /// <returns>An action result indicating the outcome of the operation.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided teacher data is invalid.</exception>
    /// <exception cref="SystemExceptionHandle">Thrown when an error occurs during the update process.</exception>
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
