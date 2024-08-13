using AnymalGrpc;

namespace AnymalApi.Services;

public static class LoggingExtensions
{
    public static Task<UpdateResponse> LogAndReturn(this ILogger logger, string message, bool success)
    {
        if (success)
        {
            logger.LogInformation(message);
        }
        else
        {
            logger.LogWarning(message);
        }
        return Task.FromResult(new UpdateResponse { Success = success, Message = message });
    }
}
