using anybotics_anymal_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace anybotics_anymal_api.Admin.Controllers
{
    /// <summary>
    /// Represents an API controller that provides administrative endpoints for managing users.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IFirebaseService _firebaseService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminController"/> class.
        /// </summary>
        /// <param name="firebaseService">The service used to interact with Firebase for user management.</param>
        public AdminController(IFirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        /// <summary>
        /// Handles HTTP GET requests to retrieve a list of all users from Firebase.
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the result of the operation.
        /// Returns 200 OK with a list of <see cref="UserInfo"/> if the request is successful.
        /// </returns>
        [HttpGet("list")]
        public async Task<IActionResult> ListUsers()
        {
            var userRecords = await _firebaseService.GetUsersAsync();
            return Ok(userRecords);
        }

        /// <summary>
        /// Handles HTTP POST requests to create a new user in Firebase with a specified role.
        /// </summary>
        /// <param name="email">The email address of the new user.</param>
        /// <param name="password">The password for the new user.</param>
        /// <param name="role">The role of the new user (e.g., admin, standard, guest).</param>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the result of the operation.
        /// Returns 201 Created if the user is created successfully.
        /// </returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password) || string.IsNullOrWhiteSpace(request.Role))
            {
                return BadRequest("Email, password, and role are required.");
            }

            var validRoles = new[] { "admin", "standard", "guest" };
            if (!validRoles.Contains(request.Role.ToLower()))
            {
                return BadRequest("Invalid role specified.");
            }

            try
            {
                var userInfo = await _firebaseService.CreateUserAsync(request.Email, request.Password, request.Role);
                return CreatedAtAction(nameof(ListUsers), new { id = userInfo.Uid }, userInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("delete/{uid}")]
        public async Task<IActionResult> DeleteUser(string uid)
        {
            if (string.IsNullOrWhiteSpace(uid))
            {
                return BadRequest("User ID is required.");
            }

            try
            {
                await _firebaseService.DeleteUserAsync(uid);
                return NoContent(); // 204 No Content if the deletion is successful
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    public record CreateUserRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    /// <summary>
    /// Represents user information with a unique identifier and email address.
    /// </summary>
    public record UserInfo
    {
        /// <summary>
        /// Gets the unique identifier for the user.
        /// </summary>
        public string Uid { get; init; }

        /// <summary>
        /// Gets the email address associated with the user.
        /// </summary>
        public string Email { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfo"/> record.
        /// </summary>
        /// <param name="uid">The unique identifier for the user.</param>
        /// <param name="email">The email address associated with the user.</param>
        public UserInfo(string uid, string email)
        {
            Uid = uid;
            Email = email;
        }
    }
}
