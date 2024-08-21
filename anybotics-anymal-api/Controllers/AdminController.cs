using anybotics_anymal_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace anybotics_anymal_api.Controllers;

public record UserInfo(string Uid, string Email);


[ApiController]
[Route("[controller]")]
public class AdminController(FirebaseService firebaseService) : ControllerBase
{
    [HttpGet("list")]
    public async Task<IActionResult> ListUsers()
    {
        var userRecords = await firebaseService.GetUsersAsync();
        return Ok(userRecords);
    }
}
