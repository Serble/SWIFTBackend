using Microsoft.AspNetCore.Mvc;
using SwiftBackend.Schemas;

namespace SwiftBackend.Controllers; 

[Controller]
[Route("authenticate")]
public class AuthenticationController : ControllerBase {
    
    [HttpPost]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticateBody body) {
        SerbleApi api = new();
        AuthCodeExchangeResponse? authCodeExchangeResponse = await api.ExchangeAuthCode(body.Token!);
        if (authCodeExchangeResponse == null) {
            return BadRequest("Invalid auth code");
        }
        
        // Are they registering or logging in?
        SerbleUser? serbleUser = await api.GetUser();
        SwiftUser? chatUser = await Program.StorageManager.GetUser(serbleUser!.Id);
        
        if (string.IsNullOrEmpty(serbleUser.Username)) {
            return BadRequest("Scope must include user_info");
        }

        if (chatUser != null) {
            if (chatUser.Username == serbleUser.Username) return Ok(authCodeExchangeResponse);
            // They changed their username
            chatUser.Username = serbleUser.Username;
            await Program.StorageManager.EditUser(chatUser);
            return Ok(authCodeExchangeResponse);
        }

        // They are registering
        chatUser = new SwiftUser {
            Id = serbleUser.Id,
            Username = serbleUser.Username,
            CreatedAt = DateTime.UtcNow.DateTimeToUnixMillis(),
            Premium = false,
            Admin = serbleUser.PermLevel == 2  // Make Serble admins Swift admins
        };
        await Program.StorageManager.CreateUser(chatUser);

        return Ok(authCodeExchangeResponse);
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshBody body) {
        SerbleApi api = new();
        AuthCodeExchangeResponse? authCodeExchangeResponse = await api.Refresh(body.RefreshToken);
        if (authCodeExchangeResponse == null) {
            return BadRequest("Invalid refresh token");
        }
        return Ok(authCodeExchangeResponse);
    }
    
    [HttpOptions]
    public ActionResult Options() {
        Response.Headers.Add("Allow", "POST, OPTIONS");
        return Ok();
    }
    
    [HttpOptions("refresh")]
    public ActionResult RefreshOptions() {
        Response.Headers.Add("Allow", "POST, OPTIONS");
        return Ok();
    }
    
}