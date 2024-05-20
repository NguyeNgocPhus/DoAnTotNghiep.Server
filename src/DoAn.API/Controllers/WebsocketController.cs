using DoAn.Application.Websocket;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.API.Controllers;

public class WebSocketController : ControllerBase
{

    private readonly WebsocketHandler _wsHandler;

    public WebSocketController(WebsocketHandler wsHandler)
    {
        _wsHandler = wsHandler;
    }

    [HttpGet("/echo")]
    public async Task Get()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            await _wsHandler.OnConnectedAsync(webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}