using anybotics_anymal_api.Controllers;
using FirebaseAdmin.Auth;

namespace anybotics_anymal_api.Services
{
    public interface IFirebaseService
    {
        Task<string> GetUidFromTokenAsync(string idToken);
        Task<string> GetUserEmailAsync(string uid);
        Task<UserRecord> GetUserRecordAsync(string uid);
        Task<string?> GetUserRoleAsync(string userEmail);
        Task<List<UserInfo>> GetUsersAsync();
    }
}