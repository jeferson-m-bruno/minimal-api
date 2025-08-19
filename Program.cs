using Microsoft.AspNetCore.Mvc;
using MinimalAPI.Domain.DTOs;

var builder = WebApplication.CreateBuilder(args);
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


