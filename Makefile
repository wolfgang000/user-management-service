migrate:
	docker compose exec web sh -c "cd UserManagemenService && /root/.dotnet/tools/dotnet-ef database update"

test:
	docker compose exec -e ASPNETCORE_CONNECTION_STRING="Host=db_dev;Database=postgres_test;Username=test_user;Password=test_password" web sh -c "cd UserManagemenService.Tests && dotnet test"
