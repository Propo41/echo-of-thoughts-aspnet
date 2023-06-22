using Cefalo.EchoOfThoughts.WebApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Cefalo.EchoOfThoughts.Domain;
using Cefalo.EchoOfThoughts.WebApi;
using Cefalo.EchoOfThoughts.AppCore.MappingProfiles;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers().AddXmlSerializerFormatters();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddAutoMapper(typeof(StoryMappingProfile), typeof(UserMappingProfile));

// DI
builder.Services.RegisterRepositories();
builder.Services.RegisterServices();

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

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.UseGlobalExceptionHandler(app.Logger);

app.Run();
