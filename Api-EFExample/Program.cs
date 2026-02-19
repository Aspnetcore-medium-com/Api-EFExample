using Core.Mapper;
using Core.Validator;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Infra;
using Services.Seeders;
using Core;

var builder = WebApplication.CreateBuilder(args);


//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
//builder.Logging.AddEventLog();
//builder.Logging.AddDebug();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<PersonAddValidator>();
builder.Services.AddCore().AddInfra(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsEnvironment("Testing"))
{
    await DbSeeder.Seed(app.Services);
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
