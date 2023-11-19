docker pull mcr.microsoft.com/mssql/server

docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Sample123$" -p 1433:1433 
--name sqlserver -d mcr.microsoft.com/mssql/server:latest