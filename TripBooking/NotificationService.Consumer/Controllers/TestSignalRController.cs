using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace NotificationService.Consumer.Controllers;

[ApiController]
[Route("[controller]")]
public class TestSignalRController : ControllerBase
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public TestSignalRController(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    [HttpGet("send")]
    public async Task<IActionResult> SendMessage(string message)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
        return Ok(new { Message = "Message sent to all clients" });
    }
}