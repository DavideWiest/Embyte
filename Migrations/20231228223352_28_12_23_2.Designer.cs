﻿// <auto-generated />
using System;
using Embyte.Modules.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Embyte.Migrations
{
    [DbContext(typeof(EmbyteDbContext))]
    [Migration("20231228223352_28_12_23_2")]
    partial class _28_12_23_2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Embyte.Data.Models.WebsiteUsage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("RequestCount")
                        .HasColumnType("integer");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("character varying(450)");

                    b.HasKey("Id");

                    b.ToTable("WebsiteUsage", (string)null);
                });

            modelBuilder.Entity("Embyte.Data.Product.WebsiteInfo", b =>
                {
                    b.Property<string>("Url")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FavIconUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("HasData")
                        .HasColumnType("boolean");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string[]>("Keywords")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("Locale")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PersonName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SiteName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SiteType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ThemeColor")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Url");

                    b.ToTable("WebsiteInfos");
                });

            modelBuilder.Entity("ExtractorEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<TimeSpan>("DeltaToPrevious")
                        .HasColumnType("interval");

                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ExtractorEntries");
                });
#pragma warning restore 612, 618
        }
    }
}
