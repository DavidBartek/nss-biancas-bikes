using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BiancasBikes.Data;

namespace BiancasBikes.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserProfileController : ControllerBase
{
    private BiancasBikesDbContext _dbContext;

    public UserProfileController(BiancasBikesDbContext context)
    {
        _dbContext = context;
    }

    [HttpGet]
    // [Authorize] // original to code, but edited below to make the method available only to logged-in admin.
    [Authorize(Roles = "Admin")]
    public IActionResult Get() // gets data for all users; should only be available to a logged-in admin.
    {
        return Ok(_dbContext.UserProfiles.ToList());
    }
}