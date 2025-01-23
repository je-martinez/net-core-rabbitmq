add-migration:
	dotnet ef migrations add $(NAME) --project NetCoreRabbitMQ.Data --startup-project NetCoreRabbitMQ.Api --context NetCoreRabbitMQ.Data.Context.ApiDbContext
run-migrations:
	dotnet ef database update --project NetCoreRabbitMQ.Data --startup-project NetCoreRabbitMQ.Api
build-images:
	docker build -t netcore-rabbitmq-api . -f ./Dockerfile.API
	docker build -t netcore-rabbitmq-worker . -f ./Dockerfile.Worker
	docker build -t netcore-rabbitmq-migrator . -f ./Dockerfile.Migrator
run-live:
	docker-compose -f ./docker-compose.live.yml up -d --build 