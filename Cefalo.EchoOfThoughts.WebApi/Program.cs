using Cefalo.EchoOfThoughts.WebApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Cefalo.EchoOfThoughts.Domain;
using Cefalo.EchoOfThoughts.WebApi;
/**
* In .NET 6 Microsoft has removed the Startup.cs class. they unified Startup.cs and Program.cs into one Program.cs.
* Just go to the program.cs 
* file and there you can add a connection string then you've to use builder.Services.AddDbContext 
* The old way is services.AddDbContext. Just use builder.Services and then you can achieve what you want.
* https://stackoverflow.com/questions/70952271/startup-cs-class-is-missing-in-net-6
*/

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddXmlSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>();
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
