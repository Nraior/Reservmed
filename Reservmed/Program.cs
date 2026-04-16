using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Reservmed.Common.Settings;
using Reservmed.Data;
using Reservmed.DTOs.Internal;
using Reservmed.Models.Identity;
using Reservmed.Services;
using Reservmed.Services.Interfaces;
using System.Threading.Channels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string"
        + "'DefaultConnection' not found.");

builder.Services.AddDbContext<ReservmedDBContext>(options => options.UseSqlServer(connectionString));
builder.Services.Configure<FrontendSettings>(
    builder.Configuration.GetSection("FrontendSettings"));
//builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Test"));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ReservmedDBContext>()
.AddDefaultTokenProviders();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailProcessorService, EmailProcessorService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IPatientService, PatientService>();

builder.Services.AddSingleton(Channel.CreateUnbounded<EmailDataDto>());

builder.Services.AddHostedService<BackgroundEmailWorker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
