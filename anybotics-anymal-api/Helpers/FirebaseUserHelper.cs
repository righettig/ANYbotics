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
            throw new Exception("Failed to retrieve user record from Firebase.", ex);
        }
    }

    public static async Task<string> GetUserEmailAsync(string uid)
    {
        var userRecord = await GetUserRecordAsync(uid);
        return userRecord.Email;
    }
}
