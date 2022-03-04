﻿// <auto-generated />
using System;
using Bulk.Test;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bulk.Test.Migrations
{
    [DbContext(typeof(TestContext))]
    partial class TestContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Bulk.Test.Model.BaseTphTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<byte>("Type")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.ToTable("BaseTphTable");

                    b.HasDiscriminator<byte>("Type");
                });

            modelBuilder.Entity("Bulk.Test.Model.SimpleTableWithIdentity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifyTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Whatever")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SimpleTableWithIdentity");
                });

            modelBuilder.Entity("Bulk.Test.Model.SimpleTableWithShadowProperty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("BoolFlag")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Description_de")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200)
                        .HasDefaultValue("DEFAULT");

                    b.Property<string>("Description_en")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<DateTime?>("ModificationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

                    b.Property<int>("State")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(2);

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("SimpleTableWithShadowProperty");
                });

            modelBuilder.Entity("Bulk.Test.Model.SimpleTableWithUri", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Uri")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SimpleTableWithUri");
                });

            modelBuilder.Entity("Bulk.Test.Model.TpHChildTableOne", b =>
                {
                    b.HasBaseType("Bulk.Test.Model.BaseTphTable");

                    b.Property<int>("ChildOneProperty")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue((byte)1);
                });

            modelBuilder.Entity("Bulk.Test.Model.TpHChildTableTwo", b =>
                {
                    b.HasBaseType("Bulk.Test.Model.BaseTphTable");

                    b.Property<int>("ChildTwoProperty")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue((byte)2);
                });
#pragma warning restore 612, 618
        }
    }
}
