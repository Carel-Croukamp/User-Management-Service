
echo "Build and Deployment"



echo Insatll donet tools
dotnet tool install --global dotnet-ef

echo Database migrations..............Begin
dotnet ef migrations add InitialMigration
echo Database migrations..............Done

echo Update database..............begin
dotnet ef database update
echo Update database..............Done

