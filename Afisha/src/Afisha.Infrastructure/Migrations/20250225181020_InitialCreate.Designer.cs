﻿// <auto-generated />
using System;
using Afisha.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Afisha.Infrastructure.Migrations
{
    [DbContext(typeof(AfishaDbContext))]
    [Migration("20250225181020_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Afisha.Domain.Entities.Event", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateOnly>("DateStart")
                        .HasColumnType("date");

                    b.Property<long>("LocationId")
                        .HasColumnType("bigint");

                    b.Property<long>("SponsorId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.HasIndex("SponsorId");

                    b.ToTable("Events");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            DateStart = new DateOnly(2025, 2, 25),
                            LocationId = 1L,
                            SponsorId = 1L
                        },
                        new
                        {
                            Id = 2L,
                            DateStart = new DateOnly(2025, 2, 25),
                            LocationId = 1L,
                            SponsorId = 1L
                        });
                });

            modelBuilder.Entity("Afisha.Domain.Entities.Location", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<bool>("IsWarmPlace")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(140)
                        .HasColumnType("character varying(140)");

                    b.Property<long>("OwnerId")
                        .HasColumnType("bigint");

                    b.Property<decimal>("Pricing")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Locations");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            IsWarmPlace = false,
                            Name = "ДК Колхозник",
                            OwnerId = 1L,
                            Pricing = 100m
                        },
                        new
                        {
                            Id = 2L,
                            IsWarmPlace = false,
                            Name = "Supr puper mega place",
                            OwnerId = 1L,
                            Pricing = 1000m
                        });
                });

            modelBuilder.Entity("Afisha.Domain.Entities.Rating", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("EventId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("Afisha.Domain.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool?>("IsMale")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Patronymic")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Email = "t@t.com",
                            FirstName = "Ivan",
                            LastName = "Ivanov",
                            Login = "Ivan",
                            PasswordHash = "1"
                        },
                        new
                        {
                            Id = 2L,
                            Email = "Petrovich@t.com",
                            FirstName = "Petr",
                            LastName = "Petrovich",
                            Login = "Petrovich",
                            PasswordHash = "1"
                        });
                });

            modelBuilder.Entity("Afisha.Domain.Entities.Event", b =>
                {
                    b.HasOne("Afisha.Domain.Entities.Location", "Location")
                        .WithMany("Events")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Afisha.Domain.Entities.User", "Sponsor")
                        .WithMany("Events")
                        .HasForeignKey("SponsorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");

                    b.Navigation("Sponsor");
                });

            modelBuilder.Entity("Afisha.Domain.Entities.Location", b =>
                {
                    b.HasOne("Afisha.Domain.Entities.User", "Owner")
                        .WithMany("Locations")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Afisha.Domain.Entities.Rating", b =>
                {
                    b.HasOne("Afisha.Domain.Entities.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Afisha.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Afisha.Domain.Entities.Location", b =>
                {
                    b.Navigation("Events");
                });

            modelBuilder.Entity("Afisha.Domain.Entities.User", b =>
                {
                    b.Navigation("Events");

                    b.Navigation("Locations");
                });
#pragma warning restore 612, 618
        }
    }
}
