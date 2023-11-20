using EntityFrameworkSqlServer;
using EntityFrameworkSqlServer.DataAccessLayer;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EntityFrameworkSqlServerContext>(options => options.UseSqlServer("name=ConnectionStrings:SqlConnection"));
builder.Services.AddScoped<DbContext, EntityFrameworkSqlServerContext>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
