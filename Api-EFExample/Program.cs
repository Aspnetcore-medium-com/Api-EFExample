using Core.Mapper;
using Core.Validator;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Infra;
using Services.Seeders;
using Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<PersonValidator>();
builder.Services.AddCore().AddInfra(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
await DbSeeder.Seed(app.Services);
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
