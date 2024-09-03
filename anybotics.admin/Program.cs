using anybotics.admin.Extensions;

var builder = WebApplication.CreateBuilder(args);

var firebaseProjectName = builder.Configuration["FIREBASE_PROJECT_NAME"];
var firebaseConfigFile = builder.Configuration["FIREBASE_CONFIG_FILE"];
var corsAllowedOrigins = builder.Configuration["CORS_ALLOWED_ORIGINS"]?.Split(',') ?? [];

// Add services to the container.
builder.Services.AddCustomServices(firebaseProjectName, firebaseConfigFile, corsAllowedOrigins);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCustomMiddleware();

app.Run();
