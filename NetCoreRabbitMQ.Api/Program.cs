using NetCoreRabbitMQ.Infrastructure.Extensions;
using NetCoreRabbitMQ.Application.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCoreRabbitMQ.Application.DTOs.Sessions;
using NetCoreRabbitMQ.Application.UseCases.Session.Commands;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("/session", async ([FromBody] CreateSessionDTO session, ISender _sender) =>
{
    var newSession = await _sender.Send(new CreateSessionCommand(session));
    if (newSession == null)
    {
        return Results.BadRequest("An error occurred while creating the session");
    }
    return Results.Created($"/session/{newSession.Id}", newSession);
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
