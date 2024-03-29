﻿// <auto-generated />
using System;
using BusinessLogic.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BusinessLogic.Data.Migrations
{
    [DbContext(typeof(BanksDBContext))]
    [Migration("20240305043828_MigracionInicial")]
    partial class MigracionInicial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.12");

            modelBuilder.Entity("Core.Entities.Banks", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("Account_number")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Bank_name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Iban")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Routing_number")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Swift_bic")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Uid")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Banks");
                });
#pragma warning restore 612, 618
        }
    }
}
