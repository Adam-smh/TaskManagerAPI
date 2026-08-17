using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using System.Text;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models.Entities;
using TaskManagerAPI.Repositories.CategoryRepository;
using TaskManagerAPI.Repositories.TaskRepository;
using TaskManagerAPI.Repositories.UserRepository;
using TaskManagerAPI.Services.Auth;
using TaskManagerAPI.Services.CategoryService;
using TaskManagerAPI.Services.TaskService;
using TaskManagerAPI.Services.UserService;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        "logs/log-.txt",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{

    Log.Information("Starting Task Manager API");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    //repositories
    builder.Services.AddScoped<ITaskRepository, TaskRepository>();
    builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();

    //services.
    builder.Services.AddScoped<ITaskService, TaskService>();
    builder.Services.AddScoped<ICategoryService, CategoryService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<ITokenService, TokenService>();

    builder.Services.AddControllers();

    builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

    var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("JWT key is missing");

    builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            )
        };
    });

    builder.Services.AddAuthorization();

    builder.Services.AddDbContext<AppDbContext>(options =>
    options
        .UseNpgsql(
            builder.Configuration
                .GetConnectionString("DefaultConnection"))
        .UseSnakeCaseNamingConvention());

    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();

        //prefer scalar
        app.MapScalarApiReference();
    }

    app.UseSerilogRequestLogging();
    
    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.UseAuthentication();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start");
}
finally
{
    Log.CloseAndFlush();
}