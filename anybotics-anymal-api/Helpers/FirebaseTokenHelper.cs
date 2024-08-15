using FirebaseAdmin.Auth;

namespace anybotics_anymal_api.Helpers;

public class FirebaseTokenHelper
{
    public static async Task<string> GetUidFromTokenAsync(string idToken)
    {
        try
        {
            var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
            return decodedToken.Uid;
        }
        catch (Exception ex)
        {
            // Handle token verification errors
            throw new Exception("Failed to verify Firebase ID token.", ex);
        }
    }
}
