using CGym.Application.Interfaces;
using CGym.Application.Services;
using CGym.Infrastructure.Persistence;
using CGym.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<GymDbContext>(
    options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// DI — fortæller ASP.NET hvilke klasser der bruges
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IMemberService, MemberService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // Swagger UI
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
