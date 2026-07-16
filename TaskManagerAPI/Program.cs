using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;
using TaskManagerAPI.Data;
using TaskManagerAPI.Repositories.CategoryRepository;
using TaskManagerAPI.Repositories.TaskRepository;
using TaskManagerAPI.Services.CategoryService;
using TaskManagerAPI.Services.TaskService;

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

    //services.
    builder.Services.AddScoped<ITaskService, TaskService>();
    builder.Services.AddScoped<ICategoryService, CategoryService>();

    builder.Services.AddControllers();

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