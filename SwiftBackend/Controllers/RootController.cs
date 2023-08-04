using Microsoft.AspNetCore.Mvc;

namespace SwiftBackend.Controllers; 

[ApiController]
[Route("/")]
public class RootController : ControllerBase {
    
    [HttpGet]
    public ActionResult Get() {
        return Ok("SWIFT Server " + Program.Version);
    }
    
}