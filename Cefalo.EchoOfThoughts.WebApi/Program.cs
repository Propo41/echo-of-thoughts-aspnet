using Cefalo.EchoOfThoughts.WebApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Cefalo.EchoOfThoughts.Domain;
using Cefalo.EchoOfThoughts.WebApi;
using Cefalo.EchoOfThoughts.AppCore.MappingProfiles;
using Cefalo.EchoOfThoughts.AppCore.Helpers;
using Cefalo.EchoOfThoughts.AppCore.Helpers.Interfaces;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// The wrapped sink (File in this case) will be invoked on a worker thread
// while your application's thread gets on with more important stuff.
// Because the memory buffer may contain events that have not yet been written
// to the target sink, it is important to call Log.CloseAndFlush() or
// Logger.Dispose() when the application exits.
var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Async(config =>
        config.File(
            "../logs/log-.txt",
            restrictedToMinimumLevel: LogEventLevel.Information,
            outputTemplate: "{Timestamp:MM-dd HH:mm:ss} {Level:u3} {Message:lj}{NewLine}",
            rollingInterval: RollingInterval.Day))
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Services
builder.Services.AddControllers(o => {
    o.ReturnHttpNotAcceptable = true;
    o.RespectBrowserAcceptHeader = true;
    o.OutputFormatters.Add(new CustomOutputFormatter());
})
    .AddXmlSerializerFormatters();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddAutoMapper(typeof(StoryMappingProfile), typeof(UserMappingProfile));
builder.Services.AddCors(o => {
    o.AddDefaultPolicy(
        policy => {
            policy
                .WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

// DI
builder.Services.RegisterRepositories();
builder.Services.RegisterServices();
builder.Services.RegisterAuthServices(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddScoped<IAuthHelper, AuthHelper>();

var app = builder.Build();

// Configure the HTTP request pipeline.     
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
    // adding migration programatically
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
} else {
    // todo for production
}

IHostApplicationLifetime lifetime = app.Lifetime;
lifetime.ApplicationStopping.Register(() => {
    Console.WriteLine("app stopping.. flushing logs");
    logger.Dispose();
});

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.UseGlobalExceptionHandler(app.Logger);

app.Run();
