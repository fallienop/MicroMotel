﻿// <auto-generated />
using System;
using MicroMotel.Services.Reservation.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MicroMotel.Services.Reservation.Migrations
{
    [DbContext(typeof(ReservationContext))]
    partial class ReservationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MicroMotel.Services.Reservation.Models.MealR", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MealId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReservationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RoomRId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoomRId");

                    b.ToTable("Meals", "Reservation");
                });

            modelBuilder.Entity("MicroMotel.Services.Reservation.Models.RoomR", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("PropertyId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReservEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ReservStart")
                        .HasColumnType("datetime2");

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.Property<decimal?>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Rooms", "Reservation");
                });

            modelBuilder.Entity("MicroMotel.Services.Reservation.Models.MealR", b =>
                {
                    b.HasOne("MicroMotel.Services.Reservation.Models.RoomR", null)
                        .WithMany("MealRs")
                        .HasForeignKey("RoomRId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MicroMotel.Services.Reservation.Models.RoomR", b =>
                {
                    b.Navigation("MealRs");
                });
#pragma warning restore 612, 618
        }
    }
}
