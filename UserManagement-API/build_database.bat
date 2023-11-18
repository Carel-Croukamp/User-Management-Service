dotnet tool install --global dotnet-ef

dotnet ef migrations add InitialMigration
dotnet ef database update