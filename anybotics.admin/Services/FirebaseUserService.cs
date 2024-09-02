using anybotics.admin.Controllers;
using FirebaseAdmin.Auth;
using Google.Cloud.Firestore;

namespace anybotics.admin.Services;

public class FirebaseUserService : IUserService
{
    private readonly FirebaseAuth _auth;
    private readonly FirestoreDb _firestoreDb;

    public FirebaseUserService(FirestoreDb firestoreDb)
    {
        _auth = FirebaseAuth.DefaultInstance;
        _firestoreDb = firestoreDb;
    }

    public async Task<List<UserInfo>> GetUsersAsync()
    {
        var userRecords = new List<UserInfo>();

        await foreach (var response in _auth.ListUsersAsync(null).AsRawResponses())
        {
            userRecords.AddRange(
                response.Users.Select(user => new UserInfo(user.Uid, user.Email)));
        }

        return userRecords;
    }

    public async Task<UserInfo> CreateUserAsync(string email, string password, string role)
    {
        try
        {
            // Create the user in Firebase Authentication
            var userRecordArgs = new UserRecordArgs
            {
                Email = email,
                Password = password,
            };
            var userRecord = await _auth.CreateUserAsync(userRecordArgs);

            // Set the user's role in Firestore
            var docRef = _firestoreDb.Collection("userRoles").Document(email);
            await docRef.SetAsync(new { role });

            return new UserInfo(userRecord.Uid, userRecord.Email);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create user in Firebase.", ex);
        }
    }

    public async Task DeleteUserAsync(string uid)
    {
        if (string.IsNullOrWhiteSpace(uid))
        {
            throw new ArgumentException("User ID cannot be null or empty.", nameof(uid));
        }

        try
        {
            // Delete the user with the given UID
            await _auth.DeleteUserAsync(uid);
        }
        catch (FirebaseAuthException ex)
        {
            // Handle Firebase-specific exceptions
            // Log or rethrow as needed
            throw new Exception($"Failed to delete user: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            // Handle other potential exceptions
            throw new Exception($"An error occurred while deleting the user: {ex.Message}", ex);
        }
    }
}
