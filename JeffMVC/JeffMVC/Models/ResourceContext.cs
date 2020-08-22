using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JeffMVC.Models
{
	public class ResourceContext : DbContext
	{
		public ResourceContext(DbContextOptions<ResourceContext> options)
				: base(options)
		{
		}

		public DbSet<ResourceEvent> ResourceEvents { get; set; }
		public DbSet<Resource> Resources { get; set; }
		public DbSet<Person> Persons { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ResourceEvent>().ToTable("ResourceEvent");
			modelBuilder.Entity<Resource>().ToTable("Resource");
			modelBuilder.Entity<Person>().ToTable("Person");
			base.OnModelCreating(modelBuilder);
		}
	}

	public class ResourceEvent
	{
		public int Id { get; set; }

		[DisplayName("Event Date")]
		[DataType(DataType.Date)]
		public DateTime EventDate { get; set; }
		public Resource Resource { get; set; }
		public Person Person { get; set; }
		public int ResourceId { get; set; }
		public int PersonId { get; set; }
	}

	public class Resource
	{
		public int Id { get; set; }
		public string ResourceTitle { get; set; }
	}

	public class Person
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	public class ResourceEventModel
	{
		public DateTime resourceEventModelDate { get; set; }
		public List<ResourceEvent> resourceEventModelBookings { get; set; }
	}
}
