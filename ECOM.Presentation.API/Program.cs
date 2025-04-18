using Serilog;
using ECOM.App.Extenstions;
using ECOM.Infrastructure.Extensions;
using ECOM.Infrastructure.Implementations.Notifications.SignalR;

using ECOM.Presentation.API.Extensions;
using ECOM.Presentation.API.Middlewares;
using ECOM.Shared.Library.Models.Settings;


var builder = WebApplication.CreateBuilder(args); 

builder.Configuration.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true);

builder.Configuration.AddEnvironmentVariables();

// Configure Global AppSettings
builder.Services.AddOptions<AppSettings>()
				.Bind(builder.Configuration.GetSection(nameof(AppSettings)))
				.ValidateDataAnnotations() 
				.ValidateOnStart();

// Add services to the container.

builder.Services.AddControllers();

// Add Jwt
builder.Services.AddJwtAuthentication(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add Logging
builder.AddLogging(builder.Configuration);

//Add Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApplicationServices();

//-----------------------------------------------------------------------------
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	// Migrate Db & Seed Data
	await app.Services.InitializeDatabaseAsync();
}

// Enable Serilog request logging
app.UseSerilogRequestLogging();

// Global exception handler
app.UseMiddleware<GlobalExceptionMiddleware>();

// Log
Log.Information("Application started successfully!");

// Configure Swagger trong Development
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// HTTPS redirection
app.UseHttpsRedirection();

// Authentication Middleware
app.UseAuthentication();

// JWT Custom Validation Middleware
app.UseMiddleware<JwtValidationMiddleware>();

// Authorization Middleware
app.UseAuthorization();

app.MapHub<NotificationHub>("/hubs/notifications");

// Map Controllers
app.MapControllers();

app.Run();

