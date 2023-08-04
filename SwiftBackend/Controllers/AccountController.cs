using Microsoft.AspNetCore.Mvc;
using SwiftBackend.Schemas;

namespace SwiftBackend.Controllers; 

[ApiController]
[Route("account")]
public class AccountController : ControllerBase {
    
    [HttpGet]
    public async Task<ActionResult<SwiftUser>> GetAccount([FromHeader] AuthenticationHeader authorization) {
        SerbleUser? serbleUser = await authorization.GetUser();
        if (serbleUser == null) {
            return Unauthorized();
        }
        SwiftUser? user = await Program.StorageManager.GetUser(serbleUser.Id);
        if (user == null) {
            return BadRequest("User does not exist");
        }
        return Ok(user);
    }
    
    [HttpOptions]
    public ActionResult Options() {
        Response.Headers.Add("Allow", "GET, OPTIONS");
        return Ok();
    }

}