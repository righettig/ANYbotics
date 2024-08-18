using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;

namespace anybotics_anymal_api.Controllers;

public record UserInfo(string Uid, string Email);


[ApiController]
[Route("[controller]")]
public class AdminController : ControllerBase
{
    private readonly FirebaseAuth _auth;

    public AdminController()
    {
        _auth = FirebaseAuth.DefaultInstance;
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListUsers()
    {
        var userRecords = new List<UserInfo>();

        await foreach (var response in FirebaseAuth.DefaultInstance.ListUsersAsync(null).AsRawResponses())
        {
            userRecords.AddRange(
                response.Users.Select(user => new UserInfo(user.Uid, user.Email)));
        }

        return Ok(userRecords);
    }
}
