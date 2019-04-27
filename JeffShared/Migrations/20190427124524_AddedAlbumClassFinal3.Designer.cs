﻿// <auto-generated />
using System;
using JeffShared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace JeffShared.Migrations
{
    [DbContext(typeof(LocationContext))]
    [Migration("20190427124524_AddedAlbumClassFinal3")]
    partial class AddedAlbumClassFinal3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("JeffShared.ViewModel.Album", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Band");

                    b.Property<string>("FileName");

                    b.Property<string>("Name");

                    b.Property<string>("Review");

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.ToTable("Album");
                });

            modelBuilder.Entity("JeffShared.ViewModel.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Category");

                    b.Property<DateTime>("DateRecorded");

                    b.Property<string>("Description");

                    b.Property<bool>("Important");

                    b.HasKey("Id");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("JeffShared.ViewModel.Member", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("MemberAlbum");
                });
#pragma warning restore 612, 618
        }
    }
}
