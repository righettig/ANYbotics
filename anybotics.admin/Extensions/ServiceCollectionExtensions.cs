using anybotics.admin.Services;
using anybotics_anymal_common.Extensions;

namespace anybotics.admin.Extensions;

// Extension methods to organize service registration and middleware configuration
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services,
                                                       string firebaseConfigFile,
                                                       string firebaseProjectName,
                                                       string[] corsAllowedOrigins)
    {
        services.ConfigureCors(corsAllowedOrigins);
        services.AddControllers();
        services.AddSwagger();

        services.AddSingleton<IUserService, FirebaseUserService>();

        services.AddFirebaseAndFirestore(firebaseConfigFile, firebaseProjectName);

        return services;
    }

    private static IServiceCollection ConfigureCors(this IServiceCollection services, string[] corsAllowedOrigins)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowApps", policyBuilder =>
            {
                policyBuilder.WithOrigins(corsAllowedOrigins)
                             .AllowAnyHeader()
                             .AllowAnyMethod()
                             .AllowCredentials();
            });
        });

        return services;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }
}