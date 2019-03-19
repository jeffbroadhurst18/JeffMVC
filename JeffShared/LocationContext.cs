using JeffShared.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JeffShared
{
	public class LocationContext : DbContext
		{
			public LocationContext(DbContextOptions<LocationContext> options)
				: base(options)
			{
			}

			public DbSet<Comment> Comments { get; set; }
			
			protected override void OnModelCreating(ModelBuilder modelBuilder)
			{
				modelBuilder.Entity<Comment>().ToTable("Comment");
				base.OnModelCreating(modelBuilder);
			}


		}
	
}
