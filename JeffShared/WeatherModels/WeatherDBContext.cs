using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace JeffShared.WeatherModels
{
    public partial class WeatherDBContext : DbContext
    {
        public WeatherDBContext()
        {
        }

        public WeatherDBContext(DbContextOptions<WeatherDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<Readings> Readings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=BROADHURST_WIN8\\BROADHURST;Database=WeatherDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Readings>(entity =>
            {
                entity.HasIndex(e => e.CityId);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Readings)
                    .HasForeignKey(d => d.CityId);
            });
        }
    }
}
