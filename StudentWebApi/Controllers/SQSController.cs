using Amazon.S3;
using Amazon.SQS;
using BLL.Services.StudentBookService;
using Microsoft.AspNetCore.Mvc;
using StudentWebApi.Models;

namespace StudentWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SQSController : ControllerBase
    {
        private readonly IStudentBookDetails _studentBookDetails;

        public SQSController(IStudentBookDetails studentBookDetails)
        {
            _studentBookDetails = studentBookDetails;
        }

        [HttpPost("Damp")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request) 
        {
            var entity = await _studentBookDetails.StudentBookDetails();
            if (entity == null)
                throw new Exception("something went wrong");

            NotificationMessage message = new()
            {
                CurrentDateTime = DateTime.Now,
                ExecutorId = request.ExecutorId,
                ActionName = request.ActionName,
                EntityId = request.EntityId,
                Body = entity

            };

        }
    }
}
