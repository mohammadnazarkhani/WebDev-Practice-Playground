using Microsoft.AspNetCore.Mvc;

namespace EFCoreShowcase.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Check()
    {
        return Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
    }
}
