using anybotics.auth.Services;
using anybotics_anymal_common.Extensions;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Google.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Firebase project configuration
var firebaseProjectName = "anybotics-c5ce9";
var firebaseApiKey = "AIzaSyBz6G-oi3GX4owL3qEl23huE5N2-zAHuco";

builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig
{
    ApiKey = firebaseApiKey,
    AuthDomain = $"{firebaseProjectName}.firebaseapp.com",
    Providers = [ new EmailProvider() ]
}));

builder.Services.AddFirebaseAndFirestore("anybotics-c5ce9-b8d42a6f97b1.json", firebaseProjectName);

// Add custom Firebase Authentication service
builder.Services.AddSingleton<IFirebaseAuthService, FirebaseAuthService>();

// Set up JWT Bearer authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://securetoken.google.com/{firebaseProjectName}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = options.Authority,
            ValidateAudience = true,
            ValidAudience = firebaseProjectName,
            ValidateLifetime = true
        };
    });

// Add controllers and Swagger for API documentation
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:4200")
                     .AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAngularApp");

app.MapControllers();

app.Run();
