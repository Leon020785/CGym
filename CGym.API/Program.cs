using CGym.Application.Interfaces;
using CGym.Application.Services;
using CGym.Infrastructure.Persistence;
using CGym.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CGym.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Henter connection string fra appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Database connection
builder.Services.AddDbContext<GymDbContext>(
    options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// ---------------- DI (Dependency Injection) ----------------

// Member system
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IActivityService, ActivityService>(); 

// User authentication system
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<AuthService>();

// ⭐ BOOKING SYSTEM (DET DU MANGLEDE)
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
builder.Services.AddScoped<ITrainerRepository, TrainerRepository>();

builder.Services.AddScoped<IBookingService, BookingService>();

// ------------------------------------------------------------


// Add controllers
builder.Services.AddControllers();

var key = builder.Configuration["Jwt:Key"];

if (string.IsNullOrEmpty(key))
{
    throw new Exception("JWT Key is missing in configuration.");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key))
    };
});

// OpenAPI (Scalar UI)
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();