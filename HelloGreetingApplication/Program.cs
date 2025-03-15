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
using System;
using RepositoryLayer.Context;


var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{
    logger.Info("Application is starting...");

    var builder = WebApplication.CreateBuilder(args);

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
    var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
    builder.Services.AddDbContext<HelloGreetingContext>(options =>
    options.UseSqlServer(connectionString));

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();


    var app = builder.Build();


    app.UseSwagger();
    app.UseSwaggerUI();


    // Configure the HTTP request pipeline.
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
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