using anybotics.auth.Services;
using Firebase.Auth;
using Firebase.Auth.Providers;
using FirebaseAdmin;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Set up environment variable for Firebase Admin SDK
Environment.SetEnvironmentVariable(
    "GOOGLE_APPLICATION_CREDENTIALS", 
    @"anybotics-c5ce9-firebase-adminsdk-7igq5-41d3e14c5b.json");

// Initialize Firebase Admin SDK
builder.Services.AddSingleton(FirebaseApp.Create());

// Firebase project configuration
var firebaseProjectName = "anybotics-c5ce9";
var firebaseApiKey = "AIzaSyBz6G-oi3GX4owL3qEl23huE5N2-zAHuco";

builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig
{
    ApiKey = firebaseApiKey,
    AuthDomain = $"{firebaseProjectName}.firebaseapp.com",
    Providers = [ new EmailProvider() ]
}));

// Initialize Firestore using the service account credentials
builder.Services.AddSingleton(FirestoreDb.Create(firebaseProjectName));

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

app.MapControllers();

app.Run();
