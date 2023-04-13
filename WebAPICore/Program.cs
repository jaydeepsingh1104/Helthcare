using Microsoft.EntityFrameworkCore;
using WebAPICore.Data;
using WebAPICore.Data.Repo;
using WebAPICore.Extensions;
using WebAPICore.Helper;
using WebAPICore.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
 builder.Services.AddScoped<IPatientRepositry,PatientRepositry>();
 
 builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
 builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
// add
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("ContactAPIConnectionString")
        ));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseAuthorization();

app.MapControllers();

app.Run();
