# Nginx Stage
FROM nginx:latest
EXPOSE 80

# Copy Nginx configuration
COPY nginx.conf /etc/nginx/nginx.conf

# ASP.NET Core Stage
FROM usermanagementwebinterface AS aspnetcore

# Copy the ASP.NET Core published output into the Nginx container
COPY --from=aspnetcore /app/wwwroot /usr/share/nginx/html