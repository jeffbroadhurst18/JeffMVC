using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JeffMVC.Models
{
	public class HolidayContext : DbContext
	{
	
		public HolidayContext(DbContextOptions<HolidayContext> options)
			: base(options)
		{
		}

		public DbSet<Holiday> Holidays { get; set; }
		

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Holiday>().ToTable("Holiday");
			base.OnModelCreating(modelBuilder);
		}


	}
}
