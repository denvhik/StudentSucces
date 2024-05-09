using AutoMapper;
using BLL.Services.StudentsDetailsService;
using BLL.Services.StudentService;
using BLL.StudentDto;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using StudentWebApi.Controllers;
using StudentWebApi.Models;

namespace StudentApiControllerTest;

[TestFixture]
public class StudentControlerTest
{
    private static readonly IStudentService service = Substitute.For<IStudentService>();
    private static readonly IMapper mapper = Substitute.For<IMapper>();
    private static readonly IStudentsDetailsService studentsDetailsService = Substitute.For<IStudentsDetailsService>();
    private readonly StudentController _studentController = new StudentController(service,mapper, studentsDetailsService);
    [SetUp]
        public void Setup()
        {
            
          
        }
   
        [Test]
        public async Task CreateNewStudent_WithValidModel_ReturnsOkResult()
        {
            // Arrange
            var studentDto = new StudentDTO
            {
                Id = 0,
                FirstName = "John",
                LastName = "Doe",
                MiddleName = "Middle",
                TicketNumber = "AA12345678",
                BirthYear = 2003,
                BirthPlace = "Kyiv",
                Address = "123 Main St",
                Gender = "male",
                MaritalStatus = "Married",
                Gmail = "johndoe@gmail.com"
            };

                service.AddStudentAsync(studentDto).Returns(Task.CompletedTask);

                // Initialize ModelState
                 _studentController.ModelState.Clear();


               // Act
                var result = await _studentController.CreateNewStudent(studentDto);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }


    [Test]
    public async Task CreateNewStudent_WithInvalidModel_ThrowsValidationException()
    {
        // Arrange
        _studentController.ModelState.AddModelError("FirstName", "Required");
        _studentController.ModelState.AddModelError("LastName", "Required");
        _studentController.ModelState.AddModelError("TicketNumber", "Required");
        _studentController.ModelState.AddModelError("BirthYear", "Required");
        _studentController.ModelState.AddModelError("Gmail", "Required");

        var studentDTO = new StudentDTO
        {
            FirstName = null,
            LastName = null,
            TicketNumber = null,
            BirthYear = 0,
            Gmail = null
        };

        // Act
        var result = await _studentController.CreateNewStudent(studentDTO);

        // Assert
        Assert.IsInstanceOf<BadRequestResult>(result);
    }

        [Test]
    public async Task GetStudent_ReturnsOkResult_WithStudentApiDtos()
    {
        // Arrange
        List<StudentDTO> studentDTOs = new List<StudentDTO>();
        service.GetStudentAsync().Returns(studentDTOs);

        List<StudentApiDto> studentApiDtos = new List<StudentApiDto>();
        mapper.Map<List<StudentApiDto>>(studentDTOs).Returns(studentApiDtos);

        // Act
        var result = await _studentController.GetStudent();

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.That(okResult.Value, Is.InstanceOf<List<StudentApiDto>>());
        var returnedStudents = okResult.Value as List<StudentApiDto>;
        Assert.That(returnedStudents, Is.EqualTo(studentApiDtos));
    }
    [Test]
    public void DeleteStudent_ThrowsArgumentException_WhenIdIsInvalid()
    {
        var invalidId = 0;
        Assert.ThrowsAsync<ArgumentException>(async () => await _studentController.DeleteStudent(invalidId));
    }
    [Test]
    public void DeleteStudent_ThrowsKeyNotFoundException_WhenStudentDoesNotExist()
    {
        var nonExistentId = 999;
        service.DeleteStudentAsync(nonExistentId).Returns(Task.FromResult(false));

        Assert.ThrowsAsync<KeyNotFoundException>(async () => await _studentController.DeleteStudent(nonExistentId));
    }
    [Test]
    public async Task DeleteStudent_ReturnsOkResult_WhenStudentIsDeletedSuccessfully()
    {
        var validId = 1;
        service.DeleteStudentAsync(validId).Returns(Task.FromResult(true));

        var result = await _studentController.DeleteStudent(validId) as OkObjectResult;

        Assert.IsNotNull(result);
        Assert.That(result.StatusCode, Is.EqualTo(200));
        Assert.That(result.Value, Is.EqualTo($"the student with id:{validId}deleted succesfuly"));
    }
}
