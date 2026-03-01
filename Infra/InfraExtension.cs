using Core.Domain.RepositoryContracts;
using Infra.Jwt;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra
{
    public static class InfraExtension
    {
        public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(configuration.GetConnectionString("Default")));
            services.AddTransient<ICountryRepository,CountryRepository>();
            services.AddTransient<IPersonRepository, PersonRepository>();
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();
            return services;
        }
    }
}
