using NetCoreRabbitMQ.Infrastructure.Extensions;
using NetCoreRabbitMQ.Application.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCoreRabbitMQ.Application.DTOs.Sessions;
using NetCoreRabbitMQ.Application.UseCases.Session.Commands;
using NetCoreRabbitMQ.Application.UseCases.Session.Queries;
using NetCoreRabbitMQ.Application.UseCases.Products.Queries;
using NetCoreRabbitMQ.Application.DTOs.Orders;
using NetCoreRabbitMQ.Application.UseCases.Orders;

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


app.MapPost("/api/order", async ([FromBody] CreateOrderDTO order, ISender _sender) =>
{
    var newOrder = await _sender.Send(new CreateOrderCommand(order));
    if (newOrder == null)
    {
        return Results.BadRequest("An error occurred while creating the order");
    }
    return Results.Created($"/api/order/{newOrder.Id}", newOrder);
}).WithName("CreateOrder");


app.MapPost("/api/session", async ([FromBody] CreateSessionDTO session, ISender _sender) =>
{
    var newSession = await _sender.Send(new CreateSessionCommand(session));
    if (newSession == null)
    {
        return Results.BadRequest("An error occurred while creating the session");
    }
    return Results.Created($"/api/session/{newSession.Id}", newSession);
}).WithName("CreateSession");

app.MapGet("/api/session/{Id}", async ([FromRoute] Guid Id, ISender _sender) =>
{
    var currentSession = await _sender.Send(new GetSessionQuery(Id));
    if (currentSession == null)
    {
        return Results.NotFound("Session not found");
    }
    return Results.Ok(currentSession);
}).WithName("GetSession");


app.MapGet("/api/products", async (ISender _sender) =>
{
    var currentSession = await _sender.Send(new GetProductsQuery());
    if (currentSession == null)
    {
        return Results.BadRequest("An error occurred while fetching the products");
    }
    return Results.Ok(currentSession);
}).WithName("GetProducts");

app.Run();
