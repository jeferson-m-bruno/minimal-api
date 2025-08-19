using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalAPI.Domain.DTOs;
using MinimalAPI.Infra.DB;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MyContext>(
    options =>
    {
        options.UseMySql(
            builder.Configuration.GetConnectionString("mysql"),
            ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
        );
    }
);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/login", ([FromBody] LoginDTO loginDto) =>
{
    if (loginDto.Email == "adm@test.com" && loginDto.Password == "123456")
    {
        return Results.Ok("Success login");
    }
    return Results.Unauthorized();
});

app.Run();


