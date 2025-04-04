using ECOM.App.Mappings.Extensions;
using ECOM.Infrastructure.Logging.Extensions;
using ECOM.Infrastructure.Persistence.Extensions;

using ECOM.Presentation.API.Middlewares;
using ECOM.Shared.Utilities.Settings;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true);

builder.Configuration.AddEnvironmentVariables();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));

// Add services to the container.

builder.Services.AddControllers();

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

var app = builder.Build();

// Enable Serilog request logging middleware
app.UseSerilogRequestLogging();

//Test log
Log.Information("Application started successfully!");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllers();

app.Run();
