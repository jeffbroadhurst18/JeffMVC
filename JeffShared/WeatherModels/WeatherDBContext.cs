using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace JeffShared.WeatherModels
{
    public partial class WeatherDBContext : DbContext, IWeatherDBContext
	{
		private readonly IConfiguration _configuration;

		public WeatherDBContext()
        {
        }

        public WeatherDBContext(DbContextOptions<WeatherDBContext> options, IConfiguration configuration)
            : base(options)
        {
			_configuration = configuration;
		}

        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<Readings> Readings { get; set; }
        public virtual DbSet<CityPairs> CityPairs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
      //      if (!optionsBuilder.IsConfigured)
      //      {
			   //optionsBuilder.UseSqlServer(_configuration.GetConnectionString("WeatherDBContext"));
      //      }
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
