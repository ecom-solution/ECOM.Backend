using ECOM.App.Mappings.Extensions;
using ECOM.App.Services.Extensions;
using ECOM.Infrastructure.Logging.Extensions;
using ECOM.Infrastructure.Persistence.Extensions;
using ECOM.Presentation.API.Extensions;
using ECOM.Presentation.API.Middlewares;
using ECOM.Shared.Utilities.Settings;
using Serilog;

var builder = WebApplication.CreateBuilder(args); 

builder.Configuration.AddEnvironmentVariables();

builder.Configuration.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true);

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

//Add for use Serilog
builder.Services.AddHttpContextAccessor();

//Add Loggings Module
builder.AddECOMLogging(builder.Configuration);

//Add Mappings Module
builder.Services.AddMappingModule();

//Add Persistence Module (Entity DbContext)
builder.Services.AddPersistence(builder.Configuration);

//Add Application Services Module
builder.Services.AddApplicationServices();

//-----------------------------------------------------------------------------

var app = builder.Build();

// Seed Data
await app.Services.SeedDatabaseAsync();

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

// Map Controllers
app.MapControllers();

app.Run();

