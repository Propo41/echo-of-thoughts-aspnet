using Cefalo.EchoOfThoughts.WebApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Cefalo.EchoOfThoughts.Domain;
using Cefalo.EchoOfThoughts.WebApi;
using Cefalo.EchoOfThoughts.AppCore.MappingProfiles;
using Cefalo.EchoOfThoughts.AppCore.Helpers;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
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

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.UseGlobalExceptionHandler(app.Logger);

app.Run();
