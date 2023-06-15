using Cefalo.EchoOfThoughts.AppCore.Infrastructure;
using Cefalo.EchoOfThoughts.WebApi.Middlewares;
using Microsoft.EntityFrameworkCore;

/**
* In .NET 6 Microsoft has removed the Startup.cs class. they unified Startup.cs and Program.cs into one Program.cs.
* Just go to the program.cs 
* file and there you can add a connection string then you've to use builder.Services.AddDbContext 
* The old way is services.AddDbContext. Just use builder.Services and then you can achieve what you want.
* https://stackoverflow.com/questions/70952271/startup-cs-class-is-missing-in-net-6
*/

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
      options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.Run();
