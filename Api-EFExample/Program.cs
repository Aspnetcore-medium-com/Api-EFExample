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
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Services;
using Services.Seeders;
using Microsoft.IdentityModel.Tokens;
using System.Text;
var builder = WebApplication.CreateBuilder(args);


//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
//builder.Logging.AddEventLog();
//builder.Logging.AddDebug();

// Add services to the container.
builder.Services.Configure<HeaderOptions>(builder.Configuration.GetSection("CustomHeaders"));
builder.Services.AddScoped(typeof(ResponseHeaderFilter));
builder.Services.AddScoped(typeof(ValidationFilter<>));
builder.Services.AddControllers(options =>
{
    options.Filters.AddService<ResponseHeaderFilter>();
});
builder.Services.AddValidatorsFromAssemblyContaining<PersonAddRequestValidator>();
builder.Services.AddCore().AddInfra(builder.Configuration);
builder.Services.AddHttpLogging(options =>{});

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services);
});
builder.Services.AddIdentity<ApplicationUser, ApplicationUserRole>( options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireDigit = false;
})
    .AddEntityFrameworkStores<ApplicationDBContext>()
    .AddDefaultTokenProviders()
    .AddUserStore<UserStore<ApplicationUser, ApplicationUserRole, ApplicationDBContext, Guid>>()
    .AddRoleStore<RoleStore<ApplicationUserRole, ApplicationDBContext, Guid>>();

// jwt
builder.Services.AddAuthentication( options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero // Recommended (removes default 5 min tolerance)
        };
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsEnvironment("Testing"))
{
    await DbSeeder.Seed(app.Services);
}
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseHttpLogging();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
