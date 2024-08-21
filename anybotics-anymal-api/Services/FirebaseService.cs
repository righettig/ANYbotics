using Google.Cloud.Firestore;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin.Auth;
using anybotics_anymal_api.Controllers;

namespace anybotics_anymal_api.Services;

public class FirebaseService : IFirebaseService
{
    private readonly FirebaseAuth _auth;
    private readonly FirestoreDb _firestoreDb;

    public FirebaseService()
    {
        string path = "anybotics-c5ce9-firebase-adminsdk-7igq5-41d3e14c5b.json";

        // Initialize Firestore using the service account credentials
        GoogleCredential credential = GoogleCredential.FromFile(path);
        FirestoreDbBuilder builder = new FirestoreDbBuilder
        {
            ProjectId = "anybotics-c5ce9",
            Credential = credential
        };
        _firestoreDb = builder.Build();

        _auth = FirebaseAuth.DefaultInstance;
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

    public async Task<UserRecord> GetUserRecordAsync(string uid)
    {
        try
        {
            return await _auth.GetUserAsync(uid);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to retrieve user record from Firebase.", ex);
        }
    }

    public async Task<string> GetUserEmailAsync(string uid)
    {
        var userRecord = await GetUserRecordAsync(uid);
        return userRecord.Email;
    }

    public async Task<string> GetUidFromTokenAsync(string idToken)
    {
        try
        {
            var decodedToken = await _auth.VerifyIdTokenAsync(idToken);
            return decodedToken.Uid;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to verify Firebase ID token.", ex);
        }
    }

    public async Task<string?> GetUserRoleAsync(string userEmail)
    {
        try
        {
            // Reference to the document in the "userRoles" collection with the specified email
            DocumentReference docRef = _firestoreDb.Collection("userRoles").Document(userEmail);

            // Get the document snapshot
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                // Retrieve the "role" field from the document
                string role = snapshot.GetValue<string>("role");
                return role;
            }
            else
            {
                Console.WriteLine("Document does not exist!");
                return null;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error fetching document: {e.Message}");
            return null;
        }
    }
}
