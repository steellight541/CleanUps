using CleanUps.BusinessDomain.Models;
using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Interfaces.PublicAccess;
using CleanUps.BusinessLogic.Services;
using CleanUps.DataAccess.DatabaseHub;
using CleanUps.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//register dependencies
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEventValidator, EventValidator>();
builder.Services.AddScoped<IEventMapper, EventMapper>();
builder.Services.AddScoped<ICRUDRepository<Event>, EventRepository>(); // Register as public interface
builder.Services.AddDbContext<CleanUpsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CleanUpsDb")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
