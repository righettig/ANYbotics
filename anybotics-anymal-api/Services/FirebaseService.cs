using Google.Cloud.Firestore;
using Google.Apis.Auth.OAuth2;

namespace anybotics_anymal_api.Services;

public class FirebaseService
{
    private FirestoreDb firestoreDb;

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
        firestoreDb = builder.Build();
    }

    public async Task<string?> GetUserRoleAsync(string userEmail)
    {
        try
        {
            // Reference to the document in the "userRoles" collection with the specified email
            DocumentReference docRef = firestoreDb.Collection("userRoles").Document(userEmail);

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
