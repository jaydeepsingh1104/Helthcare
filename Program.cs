using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebAPICore.Data;
using WebAPICore.Data.Repo;
using WebAPICore.Extensions;
using WebAPICore.Helper;
using WebAPICore.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("EnableCORS", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
builder.Services.AddControllers();
builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345")),
            ValidateAudience = false,
            ValidateIssuer = false,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = true,
        };
    });
 builder.Services.AddScoped<IPatientRepositry,PatientRepositry>();
  builder.Services.AddScoped<IUserRepositry,UserRepositry>();
 builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
 builder.Services.AddScoped<IRefreshTokenGeneratorRepository,RefreshTokenGeneratorRepository>();
 
 builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
// add
// builder.Services.AddDbContext<ApplicationDbContext>(ServiceLifetime.Transient);
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("ContactAPIConnectionString")
        ), ServiceLifetime.Transient);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.ConfigureExceptionHandler(app.Environment);    
if (app.Environment.IsDevelopment())
{
   app.UseSwagger();
                app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("EnableCORS");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
