using anybotics_anymal_api.Commands;
using anybotics_anymal_api.Commands.CommandHandlers;
using anybotics_anymal_api.Filters;
using anybotics_anymal_api.Services;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication;
using System.Reflection;

namespace anybotics_anymal_api.Extensions;


// Extension methods to organize service registration and middleware configuration
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.ConfigureCors();
        services.ConfigureAuthentication();
        services.AddGrpc();
        services.AddSignalR();
        services.AddControllersWithAuthorization();
        services.AddSwagger();

        services.AddSingleton<ICommandBus, CommandBus>();
        services.AddSingleton<ICommandRepository, InMemoryCommandRepository>();
        services.AddSingleton<AnymalService>();

        services.AddCommandHandlers(Assembly.GetExecutingAssembly());

        // Initialize Firebase Admin SDK
        FirebaseApp.Create(new AppOptions
        {
            Credential = GoogleCredential.FromFile("anybotics-c5ce9-firebase-adminsdk-7igq5-41d3e14c5b.json")
        });

        return services;
    }

    private static IServiceCollection ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAngularApp", policyBuilder =>
            {
                policyBuilder.WithOrigins("http://localhost:4200")
                             .AllowAnyHeader()
                             .AllowAnyMethod()
                             .AllowCredentials();
            });
        });

        return services;
    }

    private static IServiceCollection ConfigureAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = "CustomScheme";
            options.DefaultChallengeScheme = "CustomScheme";
            options.DefaultScheme = "CustomScheme";
        })
        .AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>("CustomScheme", options => { });

        return services;
    }

    private static IServiceCollection AddControllersWithAuthorization(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<AuthorizationFilter>();
        });

        return services;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }

    private static void AddCommandHandlers(this IServiceCollection services, Assembly assembly)
    {
        // Find all types that implement ICommandHandler<T>
        var commandHandlerTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .SelectMany(t => t.GetInterfaces(), (t, i) => new { t, i })
            .Where(ti => ti.i.IsGenericType && ti.i.GetGenericTypeDefinition() == typeof(ICommandHandler<>))
            .ToList();

        // Register each of these types as a singleton
        foreach (var handlerType in commandHandlerTypes)
        {
            services.AddSingleton(handlerType.i, handlerType.t);
        }
    }
}