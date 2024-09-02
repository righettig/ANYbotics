using anybotics.admin.Controllers;

namespace anybotics.admin.Services
{
    public interface IUserService
    {
        Task<List<UserInfo>> GetUsersAsync();
        Task<UserInfo> CreateUserAsync(string email, string password, string role);
        Task DeleteUserAsync(string uid);
    }
}