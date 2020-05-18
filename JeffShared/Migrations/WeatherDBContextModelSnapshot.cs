﻿// <auto-generated />
using System;
using JeffShared.WeatherModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace JeffShared.Migrations
{
    [DbContext(typeof(WeatherDBContext))]
    partial class WeatherDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("JeffShared.WeatherModels.Cities", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Country");

                    b.Property<string>("Name");

                    b.Property<int>("TimeLag");

                    b.HasKey("Id");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("JeffShared.WeatherModels.CityPairs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("City1Id");

                    b.Property<string>("City1Name");

                    b.Property<int>("City2Id");

                    b.Property<string>("City2Name");

                    b.HasKey("Id");

                    b.ToTable("CityPairs");
                });

            modelBuilder.Entity("JeffShared.WeatherModels.Readings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CityId");

                    b.Property<DateTime>("CurrentTime");

                    b.Property<string>("Description");

                    b.Property<string>("Direction");

                    b.Property<string>("Icon");

                    b.Property<string>("MainWeather");

                    b.Property<double>("Temperature");

                    b.Property<double>("Wind");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("Readings");
                });

            modelBuilder.Entity("JeffShared.WeatherModels.Readings", b =>
                {
                    b.HasOne("JeffShared.WeatherModels.Cities", "City")
                        .WithMany("Readings")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
