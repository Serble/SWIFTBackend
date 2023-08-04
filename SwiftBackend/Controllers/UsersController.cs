using Microsoft.AspNetCore.Mvc;
using SwiftBackend.Schemas;

namespace SwiftBackend.Controllers; 

[ApiController]
[Route("users/{userid}")]
public class UsersController : ControllerBase {
    
    [HttpGet]
    public async Task<ActionResult> GetUser([FromHeader] AuthenticationHeader authorization, [FromRoute] string userid) {
        SerbleUser? user = await authorization.GetUser();
        if (user == null) {
            return Unauthorized();
        }
        SwiftUser? chatUser = await Program.StorageManager.GetUser(userid);
        if (chatUser == null) {
            return NotFound();
        }
        return Ok(chatUser);
    }
    
}