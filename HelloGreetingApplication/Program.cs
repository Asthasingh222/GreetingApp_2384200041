using NLog;
using NLog.Web;
var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
logger.Info("Application is starting...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Set NLog as the default logger
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Adding Swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Add services to the container.

    builder.Services.AddControllers();

    var app = builder.Build();

    // Enable Swagger
    app.UseSwagger();
    app.UseSwaggerUI();

    // Configure the HTTP request pipeline.

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Application encountered a critical error and is shutting down.");
    throw;
}
finally
{
    LogManager.Shutdown();
}