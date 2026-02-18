using Infra;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTest.Integration
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(
        IWebHostBuilder builder)
        {
            //builder.ConfigureServices(services =>
            //{
            //    var descriptor = services
            //        .SingleOrDefault(
            //            d => d.ServiceType ==
            //                 typeof(DbContextOptions<ApplicationDBContext>));

            //    if (descriptor != null)
            //        services.Remove(descriptor);

            //    services.AddDbContext<ApplicationDBContext>(options =>
            //    {
            //        options.UseInMemoryDatabase("TestDb");
            //    });
            //});
            // Override config so no real connection string is picked up
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["ConnectionStrings:DefaultConnection"] = "FakeConnectionString"
                });
            });

            builder.ConfigureServices(services =>
            {
                // Remove ALL related descriptors, not just DbContextOptions
                var descriptors = services.Where(d =>
                    d.ServiceType == typeof(DbContextOptions<ApplicationDBContext>) ||
                    d.ServiceType == typeof(DbContext) ||
                    d.ServiceType == typeof(ApplicationDBContext)).ToList();

                foreach (var descriptor in descriptors)
                    services.Remove(descriptor);

                // Replace with in-memory
                services.AddDbContext<ApplicationDBContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });
            });
        }
    }
}
