using System.Reflection;
using NetCoreRabbitMQ.Infrastructure.Extensions;
using NetCoreRabbitMQ.OrdersWorker.Background;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddHostedService(provider => new OrdersWorker(provider.GetRequiredService<ILogger<OrdersWorker>>(), provider.GetRequiredService<IConfiguration>()));
var host = builder.Build();
host.Run();
