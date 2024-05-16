using AutoMapper;
using BLL.Services.StudentBookService;
using Microsoft.AspNetCore.Mvc;
using SQSSample.Models;
using SQSSample.SQSServices;
using StudentWebApi.Models;

namespace StudentWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SQSController : ControllerBase
{
    private readonly IStudentBookDetails _studentBookDetails;
    private readonly ISQSService _sQSService;
    private readonly IMapper _mapper;
    public SQSController(IStudentBookDetails studentBookDetails, ISQSService sQSService, IMapper mapper)
    {
        _studentBookDetails = studentBookDetails;
        _sQSService = sQSService;
        _mapper = mapper;
    }
    /// <summary>
    /// this method send message to queue
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [HttpPost("Queue")]
    public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
    {
        var entity = await _studentBookDetails.StudentBookDetails();
        if (entity == null)
            throw new Exception("something went wrong");

        NotificationMessageDTO message = new()
        {
            CurrentDateTime = DateTime.Now,
            ExecutorId =new Guid( request.ExecutorId).ToString(),
            ActionName = request.ActionName,
            EntityId = request.EntityId,
            Body = entity
        };
        var maprequest = _mapper.Map<NotificationMessage>(message);
        var response = await _sQSService.SendMessageAsync(maprequest);
        return Ok(response);
    }

}
