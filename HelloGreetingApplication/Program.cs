using BusinessLayer.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using RepositoryLayer;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BusinessLayer.Interface;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using RabbitMQ.Client;
using HelloGreetingApp.Helpers;


var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{
    logger.Info("Application is starting...");

    var builder = WebApplication.CreateBuilder(args);

    // Ensure that appsettings.json is loaded
    builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

    var redisConfig = builder.Configuration.GetSection("Redis");
    if (redisConfig == null || string.IsNullOrEmpty(redisConfig["ConnectionString"]))
    {
        throw new ArgumentNullException("Redis ConnectionString is missing in appsettings.json");
    }

    builder.Services.AddSingleton(new RedisCacheHelper(
        redisConfig["ConnectionString"],
        int.Parse(redisConfig["CacheTimeout"] ?? "60")  // Default timeout 60 seconds
    ));

    // Add JWT authentication services
    var jwtSettings = builder.Configuration.GetSection("Jwt");
    var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

    builder.Services.AddAuthorization();

    // Add services to the container.
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Add swagger
    builder.Services.AddControllers();
    builder.Services.AddScoped<IGreetingBL, GreetingBL>();
    builder.Services.AddScoped<IGreetingRL, GreetingRL>();
    builder.Services.AddScoped<JwtHelper>();
    builder.Services.AddScoped<EmailService>();
    // Add RabbitMQ services
    builder.Services.AddSingleton<RabbitMQProducer>();
    builder.Services.AddSingleton<RabbitMQConsumer>();
    var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
    builder.Services.AddDbContext<HelloGreetingContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();


    var app = builder.Build();

    // Start RabbitMQ Consumer in Background
    Task.Run(() => app.Services.GetRequiredService<RabbitMQConsumer>().StartListening());


    app.UseSwagger();
    app.UseSwaggerUI();


    // Configure the HTTP request pipeline.
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseRouting();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Application encountered an error and stopped.");
    throw;
}
finally
{
    LogManager.Shutdown();
}