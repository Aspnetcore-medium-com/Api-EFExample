using Entities;
//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services.Seeders
{
    /// <summary>
    /// Provides extension methods for seeding the database with initial data.
    /// </summary>
    public static class DbSeeder
    {
        /// <summary>
        /// Seeds the database with initial data for countries and persons if the corresponding tables are empty.
        /// </summary>
        /// <remarks>This method applies any pending database migrations and populates the
        /// <c>Countries</c> and <c>Persons</c> tables with data from JSON files located in the <c>jsons</c> directory
        /// under the application's current working directory. If the tables already contain data, no changes are
        /// made.</remarks>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> used to resolve the required database context and services. Cannot be
        /// <see langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous seeding operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
        public async static Task Seed(this IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }
            //using var scope = serviceProvider.CreateScope();
            //var context = scope.ServiceProvider.GetRequiredService<PersonDBContext>();
            //await context.Database.MigrateAsync();
            //if (context.Database != null) {
            //    if (!await context.Countries.AnyAsync()) {
            //        var jsonPath = Path.Combine(
            //             Directory.GetCurrentDirectory(),
            //             "jsons",
            //             "countries.json");
            //        var jsonData = await File.ReadAllTextAsync(jsonPath);
            //        var countries = JsonSerializer.Deserialize<List<Country>>(jsonData);
            //        if (countries?.Any() == true)
            //        {
            //            await context.Countries.AddRangeAsync(countries);
            //            await context.SaveChangesAsync();
            //        }
                    
            //    }
            //    if (!await context.Persons.AnyAsync())
            //    {
            //        var jsonPath = Path.Combine(Directory.GetCurrentDirectory(),
            //                    "jsons",
            //                    "persons.json");
            //        var jsonData = await File.ReadAllTextAsync($"{jsonPath}", Encoding.UTF8);
            //        var Persons = JsonSerializer.Deserialize<List<Person>>(jsonData);
            //        if(Persons?.Any() == true)
            //        {
            //            await context.Persons.AddRangeAsync(Persons);
            //            await context.SaveChangesAsync();
            //        }
            //    }
            //}

        }

    }
}
