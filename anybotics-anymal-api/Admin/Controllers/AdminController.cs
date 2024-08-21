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
