using System.Reflection;
using MediatR;
using NetCoreRabbitMQ.Infrastructure.Extensions;
using NetCoreRabbitMQ.OrdersWorker.Background;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddHostedService<OrdersWorker>();
var host = builder.Build();
host.Run();
