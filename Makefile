add-migration:
	dotnet ef migrations add $(NAME) --project NetCoreRabbitMQ.Data --startup-project NetCoreRabbitMQ
run-migrations:
	dotnet ef database update --project NetCoreRabbitMQ.Data --startup-project NetCoreRabbitMQ