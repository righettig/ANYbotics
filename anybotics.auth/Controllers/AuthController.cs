using anybotics.auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace anybotics.auth.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IFirebaseAuthService firebaseAuthService) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<IActionResult> Signup(string email, string password)
    {
        var token = await firebaseAuthService.SignUp(email, password);

        if (token is not null)
        {
            return Ok(token);
        }

        return Unauthorized();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var token = await firebaseAuthService.Login(email, password);

        if (token is not null) 
        {
            return Ok(token);
        }

        return Unauthorized();
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        firebaseAuthService.SignOut();

        return Ok();
    }

    [HttpGet("user-role")]
    public async Task<IActionResult> GetUserRole([FromQuery] string email)
    {
        var role = await firebaseAuthService.GetUserRole(email);

        if (role != null) 
        {
            return Ok(new { Role = role });
        }
        
        return NotFound();
    }
}

