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
		public DbSet<MemberAlbum> MemberAlbums { get; set; }
		public DbSet<Member> Members { get; set; }
		public DbSet<Album> Albums { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Comment>().ToTable("Comment");
			modelBuilder.Entity<Album>().ToTable("Album");
			modelBuilder.Entity<Member>().ToTable("Member");
			modelBuilder.Entity<MemberAlbum>().ToTable("MemberAlbum");
			base.OnModelCreating(modelBuilder);
		}


	}

}
