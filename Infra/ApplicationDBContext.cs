using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Country> Countries { get; set; }

        public ApplicationDBContext(DbContextOptions options):base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");
            base.OnModelCreating(modelBuilder);
            // Modify the data type, rename and set default value
            modelBuilder.Entity<Person>()
                .Property(e => e.TIN)
                .HasMaxLength(10)
                .HasColumnName("TaxIdentificationNumber")
                .HasDefaultValue("AAAA1111")
                .IsUnicode(false); // IsUnicode(false) → maps to varchar

            modelBuilder.Entity<Person>()
                .HasOne(e => e.Country)
                .WithMany(e => e.Persons)
                .HasForeignKey(e => e.CountryId);
            
            // Add Index to the country and person table
            modelBuilder.Entity<Country>()
                .HasIndex(c => c.CountryName)
                .IsUnique();
            modelBuilder.Entity<Person>()
                .HasIndex(p => p.PersonName)
                .IsUnique();

            //check constraint
            modelBuilder.Entity<Person>()
                .ToTable(t => t.HasCheckConstraint("CHK_TIN", "len([TaxIdentificationNumber]) = 8"));

        }
    }
}
