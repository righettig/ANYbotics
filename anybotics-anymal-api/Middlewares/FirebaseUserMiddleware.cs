using anybotics_anymal_api.Services;

namespace anybotics_anymal_api.Middlewares;

public class FirebaseUserMiddleware
{
    private readonly RequestDelegate _next;
    private readonly FirebaseService _firebaseService;

    public FirebaseUserMiddleware(RequestDelegate next, FirebaseService firebaseService)
    {
        _next = next;
        _firebaseService = firebaseService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var idToken = context.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

        if (!string.IsNullOrEmpty(idToken))
        {
            try
            {
                var uid = await _firebaseService.GetUidFromTokenAsync(idToken);
                var userRecord = await _firebaseService.GetUserRecordAsync(uid);

                context.Items["UserUid"] = uid;
                context.Items["UserEmail"] = userRecord.Email;

                var firebaseService = new FirebaseService();

                context.Items["UserRole"] = await _firebaseService.GetUserRoleAsync(userRecord.Email);
            }
            catch
            {
                // Handle errors or set default values
                context.Items["UserUid"] = null;
                context.Items["UserEmail"] = null;
                context.Items["UserRole"] = null;
            }
        }

        await _next(context);
    }
}
