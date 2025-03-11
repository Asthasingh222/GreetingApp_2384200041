using System.Reflection;
using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using RepositoryLayer.Content;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using Middleware;

var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
logger.Info("Application is starting...");


var builder = WebApplication.CreateBuilder(args);

    // Set NLog as the default logger
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // SQL Database Connection
    var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
    builder.Services.AddDbContext<GreetingDbContext>(options =>
     options.UseSqlServer(connectionString));

    //Adding Global Exceptions
    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<GlobalExceptionFilter>();
    });

    // Adding Swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });

    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddScoped<IGreetingRL, GreetingRL>();  // Register Repository Layer
    builder.Services.AddScoped<IGreetingBL, GreetingBL>();  // Register Business Layer

builder.Services.AddScoped<IUserRL, UserRL>();
builder.Services.AddScoped<IUserBL, UserBL>();

builder.Services.AddScoped<GlobalExceptionFilter>(); //global exception

    var app = builder.Build();

    // Enable Swagger
    app.UseSwagger();
    app.UseSwaggerUI();


    // Configure the HTTP request pipeline.

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
