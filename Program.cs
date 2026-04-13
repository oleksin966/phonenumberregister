using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PhoneNumberRegister.Data;
using PhoneNumberRegister.DTOs;
using PhoneNumberRegister.Services;
using PhoneNumberRegister.Validators;

// load .env before creating builder
var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
DotNetEnv.Env.Load(envPath);

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// PostgreSQL + EF Core — read directly from Environment
var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Services
builder.Services.AddScoped<IPhoneNumberService, PhoneNumberService>();

// Validators
builder.Services.AddScoped<IValidator<PhoneNumberRequest>, PhoneNumberValidator>();

var app = builder.Build();

// Automatically apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();