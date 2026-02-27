using Api_EFExample.Filters.Actions;
using Api_EFExample.Middleware;
using Api_EFExample.Options;
using AutoMapper;
using Core;
using Core.Domain.IdentityEntities;
using Core.Mapper;
using Core.Validator;
using FluentValidation;
using Infra;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Services;
using Services.Seeders;
var builder = WebApplication.CreateBuilder(args);


//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
//builder.Logging.AddEventLog();
//builder.Logging.AddDebug();

// Add services to the container.
builder.Services.Configure<HeaderOptions>(builder.Configuration.GetSection("CustomHeaders"));
builder.Services.AddScoped(typeof(ResponseHeaderFilter));

builder.Services.AddControllers(options =>
{
    options.Filters.AddService<ResponseHeaderFilter>();
});
builder.Services.AddValidatorsFromAssemblyContaining<PersonAddValidator>();
builder.Services.AddCore().AddInfra(builder.Configuration);
builder.Services.AddHttpLogging(options =>{});

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services);
});
builder.Services.AddIdentity<ApplicationUser, ApplicationUserRole>()
    .AddEntityFrameworkStores<ApplicationDBContext>()
    .AddDefaultTokenProviders()
    .AddUserStore<UserStore<ApplicationUser, ApplicationUserRole, ApplicationDBContext, Guid>>()
    .AddRoleStore<RoleStore<ApplicationUserRole, ApplicationDBContext, Guid>>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsEnvironment("Testing"))
{
    await DbSeeder.Seed(app.Services);
}
//if (app.Environment.IsDevelopment())
//{
    //app.UseDeveloperExceptionPage();
    app.UseExceptionHandlingMiddleware();
//}

app.UseHttpLogging();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
