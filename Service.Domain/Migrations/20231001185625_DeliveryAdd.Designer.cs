﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Service.Domain;

#nullable disable

namespace Service.Domain.Migrations
{
    [DbContext(typeof(MainContext))]
    [Migration("20231001185625_DeliveryAdd")]
    partial class DeliveryAdd
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Service.Domain.Models.DeliveryService", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DeliveryName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("DeliveryServices");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DeliveryName = "Nova Poshta"
                        },
                        new
                        {
                            Id = 2,
                            DeliveryName = "Ukr Poshta"
                        },
                        new
                        {
                            Id = 3,
                            DeliveryName = "Meest Express"
                        },
                        new
                        {
                            Id = 4,
                            DeliveryName = "Samoviviz"
                        });
                });

            modelBuilder.Entity("Service.Domain.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<decimal>("TotalPrice")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(18,2)")
                        .HasDefaultValue(0m);

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Service.Domain.Models.OrderChangeStatusLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Data")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateChange")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OldStatus")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderChangeStatusLogs");
                });

            modelBuilder.Entity("Service.Domain.Models.OrderedProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Count")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId", "OrderId")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IncludeProperties(b.HasIndex("ProductId", "OrderId"), new[] { "Count" });

                    b.ToTable("OrderedProducts");
                });

            modelBuilder.Entity("Service.Domain.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int>("Count")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<decimal>("Price")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(18,2)")
                        .HasDefaultValue(0.0m);

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Name");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Service.Domain.Models.ProductCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("ProductCategories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Food"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Technical"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Magazines"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Stationery"
                        });
                });

            modelBuilder.Entity("Service.Domain.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IncludeProperties(b.HasIndex("Login"), new[] { "PasswordHash", "PasswordSalt" });

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");

                    b.UseTphMappingStrategy();

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "admin@gmail.com",
                            Login = "Admin",
                            PasswordHash = new byte[] { 52, 55, 155, 136, 186, 220, 138, 160, 241, 88, 246, 68, 19, 226, 178, 63, 242, 123, 223, 14, 168, 124, 8, 185, 42, 235, 186, 245, 129, 200, 25, 93, 164, 11, 87, 145, 183, 177, 241, 34, 215, 119, 167, 96, 10, 253, 227, 22, 190, 62, 35, 161, 145, 240, 125, 7, 150, 238, 150, 141, 53, 6, 171, 123 },
                            PasswordSalt = new byte[] { 173, 5, 65, 66, 199, 57, 159, 41, 213, 225, 63, 201, 21, 78, 130, 72, 222, 133, 67, 144, 107, 43, 29, 70, 114, 225, 201, 56, 149, 241, 170, 101, 133, 114, 161, 95, 51, 88, 226, 33, 78, 182, 237, 131, 43, 205, 31, 178, 238, 82, 169, 0, 67, 145, 1, 44, 206, 49, 171, 222, 214, 141, 103, 153, 188, 193, 0, 125, 44, 17, 159, 160, 65, 122, 66, 112, 155, 228, 28, 203, 217, 127, 217, 247, 21, 216, 104, 144, 227, 41, 63, 85, 37, 120, 15, 200, 25, 1, 151, 230, 229, 238, 203, 166, 174, 23, 30, 121, 102, 94, 231, 105, 248, 91, 113, 135, 182, 56, 24, 37, 131, 29, 107, 159, 34, 199, 73, 207 },
                            Role = 1
                        });
                });

            modelBuilder.Entity("Service.Domain.Models.Customer", b =>
                {
                    b.HasBaseType("Service.Domain.Models.User");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.HasDiscriminator().HasValue("Customer");
                });

            modelBuilder.Entity("Service.Domain.Models.Order", b =>
                {
                    b.HasOne("Service.Domain.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Service.Domain.Models.DeliveryInfo", "Delivery", b1 =>
                        {
                            b1.Property<int>("OrderId")
                                .HasColumnType("int");

                            b1.Property<string>("Address")
                                .IsRequired()
                                .HasMaxLength(150)
                                .HasColumnType("nvarchar(150)");

                            b1.Property<int>("DeliveryServiceId")
                                .HasColumnType("int");

                            b1.HasKey("OrderId");

                            b1.HasIndex("DeliveryServiceId");

                            b1.ToTable("Orders");

                            b1.HasOne("Service.Domain.Models.DeliveryService", "DeliveryService")
                                .WithMany()
                                .HasForeignKey("DeliveryServiceId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                            b1.WithOwner()
                                .HasForeignKey("OrderId");

                            b1.Navigation("DeliveryService");
                        });

                    b.Navigation("Customer");

                    b.Navigation("Delivery")
                        .IsRequired();
                });

            modelBuilder.Entity("Service.Domain.Models.OrderChangeStatusLog", b =>
                {
                    b.HasOne("Service.Domain.Models.Order", "Order")
                        .WithMany("Changes")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Service.Domain.Models.OrderedProduct", b =>
                {
                    b.HasOne("Service.Domain.Models.Order", "Order")
                        .WithMany("OrderedProducts")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Service.Domain.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Service.Domain.Models.Product", b =>
                {
                    b.HasOne("Service.Domain.Models.ProductCategory", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Service.Domain.Models.Order", b =>
                {
                    b.Navigation("Changes");

                    b.Navigation("OrderedProducts");
                });
#pragma warning restore 612, 618
        }
    }
}
