using Amazon.SimpleNotificationService.Model;
using Microsoft.AspNetCore.Mvc;
using SNSSample.SNSservice;

namespace StudentWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SNSController : ControllerBase
    {
        private readonly ISNSService _iSNSService;

        public SNSController(ISNSService iSNSService)
        {
            _iSNSService = iSNSService;
        }
        /// <summary>
        /// this controller  sent message to NotificationSendAsync  Service and get response
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpPost("SNS")]
        public async Task<PublishResponse> PublishNotification(string message) 
        {
            try
            {
                var response = await _iSNSService.NotificationSendAsync(message);
                return response;
            } 
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException(ex.Message);
            }
        }
    }
}
