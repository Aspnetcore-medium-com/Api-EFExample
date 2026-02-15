using Core.Mapper;
using Microsoft.Extensions.DependencyInjection;
using ServiceContracts;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public static class CoreExtension
    {
        public static IServiceCollection AddCore(this IServiceCollection services) {
            Console.WriteLine("=== AutoMapper is being configured ===");

            services.AddAutoMapper(config =>
            {
                config.AddMaps(typeof(PersonMappingProfile).Assembly);
            });

            services.AddTransient<IPersonService,PersonService>();
            services.AddTransient<ICountryService,CountryService>();
            return services;
        }
    }
}
