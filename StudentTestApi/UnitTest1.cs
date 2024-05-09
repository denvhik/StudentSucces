using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace StudentTestApi
{
    [TestClass]
    public class StudentControllerTests
    {
        private Mock<IStudentService> _mockStudentService;
        private Mock<IMapper> _mockMapper;
        private StudentController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _mockStudentService = new Mock<IStudentService>();
            _mockMapper = new Mock<IMapper>();
            _controller = new StudentController(_mockStudentService.Object, _mockMapper.Object);
        }
    }
}
