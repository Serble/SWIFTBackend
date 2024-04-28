using Microsoft.AspNetCore.Mvc;

namespace SwiftBackend.Controllers; 

[ApiController]
[Route("unblock")]
public class UnblockController : ControllerBase {

    [HttpPost]
    public async Task<ActionResult> UnblockPage() {
        // TODO
        return null;
    }
    
}