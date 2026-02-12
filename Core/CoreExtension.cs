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
        public static void AddCore(this IServiceCollection services) {
            services.AddAutoMapper(cfg => { }, typeof(CoreExtension).Assembly);
            services.AddTransient<IPersonService,PersonService>();
            services.AddTransient<ICountryService,CountryService>();
        }
    }
}
