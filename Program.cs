using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StudentManagementSystem.Data;
using StudentManagementSystem.Repositories;
using StudentManagementSystem.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Veritabanı bağlantı dizesi bulunamadı! Lütfen appsettings.json dosyanızı kontrol edin.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CourseRepository>();
builder.Services.AddScoped<IGradeRepository,GradeRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StudentManagementSystem API", Version = "v1" });

});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
    Console.WriteLine("Veritabanı ve tablolar kontrol edildi veya oluşturuldu!");
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StudentManagementSystem API v1"));
}

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.UseRouting();

app.MapControllers();
app.Run();
