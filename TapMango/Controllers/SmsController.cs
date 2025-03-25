using Microsoft.AspNetCore.Mvc;
using TapMango.Services;

namespace TapMango.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SmsController : ControllerBase
    {
        private readonly RateLimiter _rateLimiter;

        public SmsController(RateLimiter rateLimiter)
        {
            _rateLimiter = rateLimiter;
        }

        [HttpPost("can-send")]
        public IActionResult CanSend([FromBody] SmsRequest request)
        {
            if (_rateLimiter.CanSendMessage(request.PhoneNumber))
            {
                return Ok(new { CanSend = true });
            }
            else
            {
                return Ok(new { CanSend = false });
            }
        }
    }

    public class SmsRequest
    {
        public string PhoneNumber { get; set; }
    }
}