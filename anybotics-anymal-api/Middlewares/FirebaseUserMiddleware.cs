using anybotics_anymal_api.Helpers;
using anybotics_anymal_api.Services;

namespace anybotics_anymal_api.Middlewares;

public class FirebaseUserMiddleware
{
    private readonly RequestDelegate _next;

    public FirebaseUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var idToken = context.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

        if (!string.IsNullOrEmpty(idToken))
        {
            try
            {
                var uid = await FirebaseTokenHelper.GetUidFromTokenAsync(idToken);
                var userRecord = await FirebaseUserHelper.GetUserRecordAsync(uid);

                context.Items["UserUid"] = uid;
                context.Items["UserEmail"] = userRecord.Email;

                var firebaseService = new FirebaseService();

                context.Items["UserRole"] = await firebaseService.GetUserRoleAsync(userRecord.Email);
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
