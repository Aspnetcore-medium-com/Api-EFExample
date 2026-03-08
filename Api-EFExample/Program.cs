using Api_EFExample.Filters.Actions;
using Api_EFExample.Options;
using Core;
using Core.Domain.IdentityEntities;
using Core.Validator;
using FluentValidation;
using Infra;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Serilog;
using Services;
using Services.Seeders;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Microsoft.OpenApi;
using Microsoft.Extensions.Configuration;

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
    options.Filters.Add(new ProducesAttribute("application/json"));
    options.Filters.Add(new ConsumesAttribute("application/json"));
});
builder.Services.AddValidatorsFromAssemblyContaining<PersonAddRequestValidator>();
builder.Services.AddCore().AddInfra(builder.Configuration);
builder.Services.AddHttpLogging(options =>{});
// api versioning
builder.Services.AddApiVersioning( config  =>
{
    //config.ApiVersionReader = new UrlSegmentApiVersionReader();
    config.ApiVersionReader = new QueryStringApiVersionReader("api-version");
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    //"api-version"

}).AddMvc()        
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "app.xml"));
    options.SwaggerDoc("v1", new OpenApiInfo   // ← No longer under .Models
    {
        Version = "v1",
        Title = "Persons API v1",
        Description = "API for managing persons (version 1)"
    });
    options.SwaggerDoc("v2", new OpenApiInfo   // ← No longer under .Models
    {
        Version = "v2",
        Title = "Persons API v2",
        Description = "API for managing persons (version 2)"
    });
});

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

// enable authentication for all controllers
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
                                    .RequireAuthenticatedUser().Build();
});

// enable cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder => policyBuilder.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()!)
            .WithHeaders("Authorization","origin","accept","content-type")
            .WithMethods("GET","POST","PUT","DELETE")
    );

    //custom policy
    options.AddPolicy("ReactClient", policyBuilder => policyBuilder.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()!)
            .WithHeaders("Authorization", "origin", "accept", "content-type")
            .WithMethods("GET", "POST", "PUT", "DELETE"));
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
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Persons API v1");
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "Persons API v2");
});
app.UseHttpLogging();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
