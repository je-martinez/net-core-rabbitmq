add-migration:
	dotnet ef migrations add $(NAME) --project NetCoreRabbitMQ.Data --startup-project NetCoreRabbitMQ.Api --context NetCoreRabbitMQ.Data.Context.ApiDbContext
run-migrations:
	dotnet ef database update --project NetCoreRabbitMQ.Data --startup-project NetCoreRabbitMQ.Api