using anybotics_anymal_api.Filters;
using anybotics_anymal_api.Hubs;
using anybotics_anymal_api.Middlewares;
using AnymalApi.Services;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", builder =>
    {
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});
builder.Services.AddGrpc();
builder.Services.AddSignalR();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<AuthorizationFilter>();
});

// Configure basic authentication (this can be a placeholder if you use custom middleware)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "CustomScheme";
    options.DefaultChallengeScheme = "CustomScheme";
    options.DefaultScheme = "CustomScheme";
})
.AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>("CustomScheme", options => { });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<AnymalService>();

// Initialize Firebase Admin SDK
FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromFile("anybotics-c5ce9-firebase-adminsdk-7igq5-41d3e14c5b.json")
});

var app = builder.Build();

// Custom Firebase authentication middleware
app.Use(async (context, next) =>
{
    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

    if (token != null)
    {
        try
        {
            var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
            context.Items["FirebaseUser"] = decodedToken; // Store the decoded token or user info
        }
        catch (Exception)
        {
            // Handle token verification failure
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Invalid Firebase token.");
            return;
        }
    }

    await next();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseMiddleware<FirebaseUserMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowAngularApp");

app.MapGrpcService<AnymalService>();
app.MapHub<AgentsHub>("/agentsHub").RequireCors("AllowAngularApp");
app.MapControllers();

app.Run();
