using AutoMapper;
using BLL.Services.StudentService;
using Microsoft.AspNetCore.Http;
using Moq;
using StudentWebApi.Controllers;
using System.Net.Http;

namespace StudentApiTest
{
    public class StudentControllerTest
    {
        private readonly Mock<IStudentService> _mockStudentService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly StudentController _controller;

        public StudentControllerTest()
        {
            _mockStudentService = new Mock<IStudentService>();
            _mockMapper = new Mock<IMapper>();
            _mockHttpContext = new Mock<HttpContext>();
            _controller = new StudentController(_mockStudentService.Object, _mockMapper.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = _mockHttpContext.Object
                }
            };
        }
    }
}