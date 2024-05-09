using Asp.Versioning;
using AutoMapper;
using BLL.Services.StudentsDetailsService;
using BLL.Services.StudentService;
using BLL.StudentDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
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
    private readonly IMapper _mapper;
    private readonly IStudentsDetailsService _studentsDetailsService;

    public StudentController(IStudentService studentService, IMapper mapper, IStudentsDetailsService studentsDetailsService)
    {
        _studentService = studentService;
        _mapper = mapper;
        _studentsDetailsService = studentsDetailsService;
    }
    /// <summary>
    /// Retrieves a filtered and sorted list of students based on the provided Sieve model.
    /// this is separate library installed using packege nuggets menager that allows filtering 
    /// </summary>
    /// <returns>A filtered and sorted list of students.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no students are found matching the filter criteria.</exception>
    [HttpGet("GetStudent")]
    //[Authorize(Roles = "Admin,Member,Menager")]
    public async Task<IActionResult> GetStudent()
    {
        List<StudentDTO> studentDTOs = new List<StudentDTO>();

        studentDTOs = await _studentService.GetStudentAsync();
        var studentresult = _mapper.Map<List<StudentApiDto>>(studentDTOs);
        if (studentresult == null)
        {
            throw new KeyNotFoundException();
        }
        
        return Ok(studentresult);
    }

    /// <summary>
    /// Retrieves students based on search, sorting parameters, and pagination details.
    /// This is custom created method for pagination filtering and sorting student 
    /// </summary>
    /// <param name="term">Search term for filtering students.</param>
    /// <param name="sort">Sorting parameter.</param>
    /// <param name="page">Page number for pagination.</param>
    /// <param name="limit">Number of records per page.</param>
    /// <returns>A paginated list of students according to search and sorting parameters.
    /// Also returns the number of all records in the header,
    /// and calculates the number of displayed pages</returns>
    [HttpGet("StudentSortingFiltering")]
    [Authorize(Roles = "Admin,Member,Menager")]
    public async Task<IActionResult> GetStudentStudentSortingFiltering(string term, string sort, int page = 1, int limit = 10)
    {
        var results = await _studentService.GetSortingEntity(term, sort, page, limit);

        // AddAsync pagination headers to the response
        Response.Headers.Append("X-Total-Count", results.TotalCount.ToString());
        Response.Headers.Append("X-Total-Pages", results.TotalPages.ToString());
        return Ok(results);
    }
    /// <summary>
    /// Retrieves a specified range of students.
    /// </summary>
    /// <param name="skip">The number of students to skip.</param>
    /// <param name="take">The number of students to return.</param>
    /// <returns>A list of students based on the specified range.</returns>
    /// <exception cref="ArgumentException">Thrown when the skip or take parameters are less than or equal to zero.</exception>
    /// <exception cref="CustomException">Thrown when an unexpected error occurs during the operation.</exception>
    [HttpGet("GetByParametrStudent")]
    [Authorize(Roles = "Admin,Member,Menager")]
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
    /// <summary>
    /// Creates a new student with the provided student details.
    /// </summary>
    /// <param name="studentDTO">The student data transfer object containing the new student details.</param>
    /// <returns>A success result if the student is successfully created.</returns>
    /// <exception cref="ValidationException">Thrown when the input model validation fails.</exception>
    /// <exception cref="CustomException">Thrown when an unexpected error occurs during the creation process.</exception>
    [HttpPost("Create")]
    [Authorize(Roles = "Admin,Menager,Member")]

    public async Task<ActionResult> CreateNewStudent([FromBody] StudentDTO studentDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest();
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
    /// Updates a student's details in the database.
    /// </summary>
    /// <param name="studentDTO">The student data transfer object containing the updated information.</param>
    /// <returns>Returns an Ok result with a success message if the student was updated successfully.</returns>
    /// <exception cref="ValidationException">Thrown when the input model validation fails or if any other validation errors occur during the update process.</exception>
    [HttpPut("Update")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> UpdateStudent([FromBody] StudentDTO studentDTO)
    {

        if (ModelState.IsValid)
            return  BadRequest();
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
    /// <summary>
    /// Deletes a student from the database based on the specified ID.
    /// </summary>
    /// <param name="id">The ID of the student to be deleted.</param>
    /// <returns>Returns a success message if the deletion was successful.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided ID is less than or equal to zero.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if no student is found with the provided ID or if the deletion failed.</exception>
    [HttpDelete("DeleteStudent")]
    [Authorize(Roles = "Admin,Menager")]
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
    /// <summary>
    /// Manages book assignments to a student. If a time is provided, it processes the returning of a book, otherwise, it issues a book to the student.
    /// </summary>
    /// <param name="studentId">The ID of the student to whom the book is issued or from whom it is returned.</param>
    /// <param name="bookId">The ID of the book to issue or return.</param>
    /// <param name="time">The optional time when the book was returned.</param>
    /// <returns>Returns a success message regarding the book transaction.</returns>
    /// <exception cref="CustomException">Thrown when an error occurs during the book management process.</exception>
    /// <exception cref="Exception">General exception for unexpected errors.</exception>
    [HttpPost("BookMenagment")]
    [Authorize(Roles = "Admin,Member,Menager")]
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
    /// <summary>
    /// Adds one or more students to a specified dormitory.
    /// </summary>
    /// <param name="studentId">A list of student IDs to be added to the dormitory.</param>
    /// <param name="dormitoryId">The ID of the dormitory where the students will be added.</param>
    /// <returns>Returns a success message or the result of the addition process.</returns>
    [HttpPost("InsertStudentToDormitory")]
    [Authorize(Roles = "Menager")]
    public async Task<ActionResult<string>> AddSrudentToDormitory([FromBody] List<int> studentId, int? dormitoryId)
    {
        var result = await _studentService.CallInsertStudentsDormitoryProcedureAsync(studentId, dormitoryId);
        return Ok(result);
    }
    /// <summary>
    /// Partially updates student information using JSON Patch.
    /// </summary>
    /// <param name="studentId">The ID of the student to update.</param>
    /// <param name="patchdocument">The JSON Patch document describing updates to the student.</param>
    /// <returns>Returns a success message if the update was successful.</returns>
    /// <exception cref="ValidationException">Thrown if the JSON Patch document is invalid.</exception>
    [HttpPatch("UpdatePartial/{studentId}")]
    [Authorize(Roles = "Menager")]
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
    /// <summary>
    /// Retrieves detailed information for a student, potentially including related entities based on include parameters.
    /// </summary>
    /// <param name="id">The student's ID for which information is requested.</param>
    /// <param name="includes">Optional parameters to specify which related entities to include in the response (e.g., "Courses").</param>
    /// <returns>Returns detailed information about the student.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if no student is found with the given ID.</exception>
    [HttpGet("GetStudentInfo/{id:int}")]
    [Authorize(Roles = "Admin,Member,Menager")]
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