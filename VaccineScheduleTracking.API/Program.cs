using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VaccineScheduleTracking.API.Data;
using VaccineScheduleTracking.API.Helpers;
using VaccineScheduleTracking.API.Mappings;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Services;
using VaccineScheduleTracking.API_Test.Repository.Accounts;
using VaccineScheduleTracking.API_Test.Repository.Appointments;
using VaccineScheduleTracking.API_Test.Repository.Children;
using VaccineScheduleTracking.API_Test.Repository.DailyTimeSlots;
using VaccineScheduleTracking.API_Test.Repository.Record;
using VaccineScheduleTracking.API_Test.Repository.VaccinePackage;
using VaccineScheduleTracking.API_Test.Repository.Vaccines;
using VaccineScheduleTracking.API_Test.Services.Accounts;
using VaccineScheduleTracking.API_Test.Services.Appointments;
using VaccineScheduleTracking.API_Test.Services.Children;
using VaccineScheduleTracking.API_Test.Services.DailyTimeSlots;
using VaccineScheduleTracking.API_Test.Services.Record;
using VaccineScheduleTracking.API_Test.Services.VaccinePackage;
using VaccineScheduleTracking.API_Test.Services.Vaccines;
using VaccineScheduleTracking.API_Test.Helpers;
using VaccineScheduleTracking.API_Test.Repository.Doctors;
using VaccineScheduleTracking.API_Test.Services.Doctors;
using VaccineScheduleTracking.API_Test.Repository.VaccineContainers;
using VaccineScheduleTracking.API_Test.Configurations;
using Microsoft.Extensions.Options;
using VaccineScheduleTracking.API_Test.Payments.VnPay.Service;
using VaccineScheduleTracking.API_Test.Payments.VnPay.Repository;
using VaccineScheduleTracking.API_Test.Payment.VnPay.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Dinh nghia bear token
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer {token}'.Ex: 'Bearer abc123xyz'"
    });

    // Yeu cau su dung token cho cac Api
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<VaccineScheduleDbContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IDailyScheduleRepository, SQLDailyScheduleRepository>();
builder.Services.AddScoped<IDailyScheduleService, DailyScheduleService>();
builder.Services.AddScoped<ITimeSlotServices, TimeSlotServices>();
builder.Services.AddScoped<ITimeSlotRepository, SQLTimeSlotRepository>();
builder.Services.AddScoped<IDoctorServices, DoctorServices>();
builder.Services.AddScoped<IDoctorRepository, SQLDoctorRepository>();
builder.Services.AddScoped<IAppointmentRepository, SQLAppointmentReopsitory>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
builder.Services.AddScoped<IAccountRepository, SQLAccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IVaccineRepository, SQLVaccineRepository>();
builder.Services.AddScoped<IVaccineService, VaccineService>();
builder.Services.AddScoped<IChildRepository, SQLChildRepository>();
builder.Services.AddScoped<IChildService, ChildService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
builder.Services.AddScoped<IVaccineRecordRepository, SQLVaccineRecordRepository>();
builder.Services.AddScoped<IVaccineRecordService, VaccineRecordService>();
builder.Services.AddScoped<IVaccineComboRepository, SQLVaccineComboRepository>();
builder.Services.AddScoped<IVaccineComboService, VaccineComboService>();
builder.Services.AddScoped<IVaccineContainerRepository, SQLVaccineContainerRepository>();

builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IVnPayService, VnPayService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
    };
});

builder.Services.AddScoped<JwtHelper>();

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

//set đường dẫn cho file log.txt (giống SQL DB)
ExceptionHelper.Configure(builder.Configuration);

//cấu hình file DefaultConfig
builder.Services.Configure<DefaultConfig>(builder.Configuration.GetSection("DefaultConfig"));
builder.Services.AddSingleton<DefaultConfig>(sp => sp.GetRequiredService<IOptions<DefaultConfig>>().Value);

builder.Services.AddSingleton<TimeSlotHelper>();

//đăng kí chạy background
builder.Services.AddHostedService<ScheduledTaskService>();
builder.Host.UseWindowsService();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();
