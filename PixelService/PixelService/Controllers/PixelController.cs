using Microsoft.AspNetCore.Mvc;
using PixelService.Models;
using PixelService.Services.Interface;

namespace PixelService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PixelController : ControllerBase
    {
        private readonly IResourceServices _resourceServices;
        private readonly IMessageProducer _messagePublisher;

        public PixelController(IResourceServices resourceServices, IMessageProducer messagePublisher)
        {
            _resourceServices = resourceServices;
            _messagePublisher = messagePublisher;
        }

        [HttpGet("track")]
        public async Task<IActionResult> Tarck()
        {

            TrackModel trackModel = new TrackModel();
            trackModel.ReferrerHeader = Request.Headers["Referer"].ToString();
            trackModel.UserAgentHeader = Request.Headers["User-Agent"].ToString();
            trackModel.VisitorIPAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            _messagePublisher.SendMessage(trackModel);

            var gifFile = await _resourceServices.GetGifByDefaultPathAsync();

            return File(gifFile, "image/jpeg");
        }
    }
}
