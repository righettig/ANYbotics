using FirebaseAdmin.Auth;

namespace anybotics_anymal_api.Helpers;

public class FirebaseUserHelper
{
    public static async Task<UserRecord> GetUserRecordAsync(string uid)
    {
        try
        {
            return await FirebaseAuth.DefaultInstance.GetUserAsync(uid);
        }
        catch (Exception ex)
        {
            // Handle errors
            throw new Exception("Failed to retrieve user record from Firebase.", ex);
        }
    }
}
