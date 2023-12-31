﻿// <auto-generated />
using System;
using AguasNico.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AguasNico.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231231211413_ReturnedProductType")]
    partial class ReturnedProductType
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AguasNico.Models.Cart", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ID"));

                    b.Property<long>("ClientID")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsStatic")
                        .HasColumnType("bit");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<long>("RouteID")
                        .HasColumnType("bigint");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("ClientID");

                    b.HasIndex("RouteID");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("AguasNico.Models.CartPaymentMethod", b =>
                {
                    b.Property<long>("CartID")
                        .HasColumnType("bigint");

                    b.Property<short>("PaymentMethodID")
                        .HasColumnType("smallint");

                    b.Property<decimal>("Amount")
                        .HasColumnType("money");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("CartID", "PaymentMethodID");

                    b.HasIndex("PaymentMethodID");

                    b.ToTable("CartPaymentMethods");
                });

            modelBuilder.Entity("AguasNico.Models.CartProduct", b =>
                {
                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<long>("CartID")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("SettedPrice")
                        .HasColumnType("money");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Type", "CartID");

                    b.HasIndex("CartID");

                    b.ToTable("CartProducts");
                });

            modelBuilder.Entity("AguasNico.Models.Client", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ID"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("CUIT")
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("DealerID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("Debt")
                        .HasColumnType("money");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DeliveryDay")
                        .HasColumnType("int");

                    b.Property<bool>("HasInvoice")
                        .HasColumnType("bit");

                    b.Property<int?>("InvoiceType")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Observations")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int?>("TaxCondition")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("DealerID");

                    b.ToTable("Clients");

                    b.HasData(
                        new
                        {
                            ID = 1L,
                            Address = "Rivadavia 1097",
                            CUIT = "20123123127",
                            CreatedAt = new DateTime(2023, 12, 31, 18, 14, 13, 303, DateTimeKind.Utc).AddTicks(4825),
                            Debt = 1500m,
                            HasInvoice = true,
                            InvoiceType = 2,
                            Name = "Martín Sola",
                            Observations = "Cuidado con el perro",
                            Phone = "3404123123",
                            TaxCondition = 2,
                            UpdatedAt = new DateTime(2023, 12, 31, 18, 14, 13, 303, DateTimeKind.Utc).AddTicks(4825)
                        },
                        new
                        {
                            ID = 2L,
                            Address = "A la vuelta de la cristalería",
                            CreatedAt = new DateTime(2023, 12, 31, 18, 14, 13, 303, DateTimeKind.Utc).AddTicks(4831),
                            Debt = 0m,
                            HasInvoice = false,
                            Name = "Agustín Bettig",
                            Phone = "3404123123",
                            UpdatedAt = new DateTime(2023, 12, 31, 18, 14, 13, 303, DateTimeKind.Utc).AddTicks(4831)
                        });
                });

            modelBuilder.Entity("AguasNico.Models.ClientProduct", b =>
                {
                    b.Property<long>("ProductID")
                        .HasColumnType("bigint");

                    b.Property<long>("ClientID")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("ProductID", "ClientID");

                    b.HasIndex("ClientID");

                    b.ToTable("ClientProducts");
                });

            modelBuilder.Entity("AguasNico.Models.DispatchedProduct", b =>
                {
                    b.Property<long>("RouteID")
                        .HasColumnType("bigint");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("RouteID", "Type");

                    b.ToTable("DispatchedProducts");
                });

            modelBuilder.Entity("AguasNico.Models.Expense", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ID"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("money");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("AguasNico.Models.PaymentMethod", b =>
                {
                    b.Property<short>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<short>("ID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("PaymentMethods");

                    b.HasData(
                        new
                        {
                            ID = (short)1,
                            Name = "Efectivo"
                        },
                        new
                        {
                            ID = (short)2,
                            Name = "Transferencia"
                        },
                        new
                        {
                            ID = (short)3,
                            Name = "Mercado Pago"
                        });
                });

            modelBuilder.Entity("AguasNico.Models.Product", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<decimal>("Price")
                        .HasColumnType("money");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            ID = 1L,
                            CreatedAt = new DateTime(2023, 12, 31, 18, 14, 13, 303, DateTimeKind.Utc).AddTicks(4688),
                            Name = "Máquina frío/calor",
                            Price = 7800m,
                            Type = 4,
                            UpdatedAt = new DateTime(2023, 12, 31, 18, 14, 13, 303, DateTimeKind.Utc).AddTicks(4693)
                        },
                        new
                        {
                            ID = 2L,
                            CreatedAt = new DateTime(2023, 12, 31, 18, 14, 13, 303, DateTimeKind.Utc).AddTicks(4698),
                            Name = "B12L",
                            Price = 1800m,
                            Type = 2,
                            UpdatedAt = new DateTime(2023, 12, 31, 18, 14, 13, 303, DateTimeKind.Utc).AddTicks(4698)
                        },
                        new
                        {
                            ID = 3L,
                            CreatedAt = new DateTime(2023, 12, 31, 18, 14, 13, 303, DateTimeKind.Utc).AddTicks(4699),
                            Name = "B20L",
                            Price = 2400m,
                            Type = 1,
                            UpdatedAt = new DateTime(2023, 12, 31, 18, 14, 13, 303, DateTimeKind.Utc).AddTicks(4699)
                        },
                        new
                        {
                            ID = 4L,
                            CreatedAt = new DateTime(2023, 12, 31, 18, 14, 13, 303, DateTimeKind.Utc).AddTicks(4700),
                            Name = "Soda 1/2",
                            Price = 600m,
                            Type = 3,
                            UpdatedAt = new DateTime(2023, 12, 31, 18, 14, 13, 303, DateTimeKind.Utc).AddTicks(4700)
                        },
                        new
                        {
                            ID = 5L,
                            CreatedAt = new DateTime(2023, 12, 31, 18, 14, 13, 303, DateTimeKind.Utc).AddTicks(4701),
                            Name = "B20L BAJADO",
                            Price = 2800m,
                            Type = 1,
                            UpdatedAt = new DateTime(2023, 12, 31, 18, 14, 13, 303, DateTimeKind.Utc).AddTicks(4702)
                        },
                        new
                        {
                            ID = 6L,
                            CreatedAt = new DateTime(2023, 12, 31, 18, 14, 13, 303, DateTimeKind.Utc).AddTicks(4702),
                            Name = "B20L",
                            Price = 1331m,
                            Type = 1,
                            UpdatedAt = new DateTime(2023, 12, 31, 18, 14, 13, 303, DateTimeKind.Utc).AddTicks(4703)
                        },
                        new
                        {
                            ID = 7L,
                            CreatedAt = new DateTime(2023, 12, 31, 18, 14, 13, 303, DateTimeKind.Utc).AddTicks(4703),
                            Name = "Dispenser",
                            Price = 3500m,
                            Type = 4,
                            UpdatedAt = new DateTime(2023, 12, 31, 18, 14, 13, 303, DateTimeKind.Utc).AddTicks(4704)
                        },
                        new
                        {
                            ID = 8L,
                            CreatedAt = new DateTime(2023, 12, 31, 18, 14, 13, 303, DateTimeKind.Utc).AddTicks(4705),
                            Name = "MAQUINA SIN CARGO",
                            Price = 0m,
                            Type = 4,
                            UpdatedAt = new DateTime(2023, 12, 31, 18, 14, 13, 303, DateTimeKind.Utc).AddTicks(4705)
                        },
                        new
                        {
                            ID = 9L,
                            CreatedAt = new DateTime(2023, 12, 31, 18, 14, 13, 303, DateTimeKind.Utc).AddTicks(4706),
                            Name = "B20L SIN CARGO",
                            Price = 0m,
                            Type = 1,
                            UpdatedAt = new DateTime(2023, 12, 31, 18, 14, 13, 303, DateTimeKind.Utc).AddTicks(4706)
                        },
                        new
                        {
                            ID = 10L,
                            CreatedAt = new DateTime(2023, 12, 31, 18, 14, 13, 303, DateTimeKind.Utc).AddTicks(4706),
                            Name = "B20L",
                            Price = 2000m,
                            Type = 1,
                            UpdatedAt = new DateTime(2023, 12, 31, 18, 14, 13, 303, DateTimeKind.Utc).AddTicks(4707)
                        },
                        new
                        {
                            ID = 11L,
                            CreatedAt = new DateTime(2023, 12, 31, 18, 14, 13, 303, DateTimeKind.Utc).AddTicks(4730),
                            Name = "B20L",
                            Price = 1800m,
                            Type = 1,
                            UpdatedAt = new DateTime(2023, 12, 31, 18, 14, 13, 303, DateTimeKind.Utc).AddTicks(4730)
                        });
                });

            modelBuilder.Entity("AguasNico.Models.ReturnedProduct", b =>
                {
                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<long>("CartID")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Type", "CartID");

                    b.HasIndex("CartID");

                    b.ToTable("ReturnedProducts");
                });

            modelBuilder.Entity("AguasNico.Models.Route", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsStatic")
                        .HasColumnType("bit");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("AguasNico.Models.Transfer", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ID"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("money");

                    b.Property<long>("ClientID")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ID");

                    b.HasIndex("ClientID");

                    b.HasIndex("UserID");

                    b.ToTable("Transfers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("nvarchar(21)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("AguasNico.Models.ApplicationUser", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.Property<int?>("TruckNumber")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("ApplicationUser");
                });

            modelBuilder.Entity("AguasNico.Models.Cart", b =>
                {
                    b.HasOne("AguasNico.Models.Client", "Client")
                        .WithMany("Carts")
                        .HasForeignKey("ClientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AguasNico.Models.Route", "Route")
                        .WithMany("Carts")
                        .HasForeignKey("RouteID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Route");
                });

            modelBuilder.Entity("AguasNico.Models.CartPaymentMethod", b =>
                {
                    b.HasOne("AguasNico.Models.Cart", "Cart")
                        .WithMany("PaymentMethods")
                        .HasForeignKey("CartID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AguasNico.Models.PaymentMethod", "PaymentMethod")
                        .WithMany("Carts")
                        .HasForeignKey("PaymentMethodID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");

                    b.Navigation("PaymentMethod");
                });

            modelBuilder.Entity("AguasNico.Models.CartProduct", b =>
                {
                    b.HasOne("AguasNico.Models.Cart", "Cart")
                        .WithMany("Products")
                        .HasForeignKey("CartID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");
                });

            modelBuilder.Entity("AguasNico.Models.Client", b =>
                {
                    b.HasOne("AguasNico.Models.ApplicationUser", "Dealer")
                        .WithMany()
                        .HasForeignKey("DealerID");

                    b.Navigation("Dealer");
                });

            modelBuilder.Entity("AguasNico.Models.ClientProduct", b =>
                {
                    b.HasOne("AguasNico.Models.Client", "Client")
                        .WithMany("Products")
                        .HasForeignKey("ClientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AguasNico.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("AguasNico.Models.DispatchedProduct", b =>
                {
                    b.HasOne("AguasNico.Models.Route", "Route")
                        .WithMany("DispatchedProducts")
                        .HasForeignKey("RouteID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Route");
                });

            modelBuilder.Entity("AguasNico.Models.Expense", b =>
                {
                    b.HasOne("AguasNico.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("AguasNico.Models.ReturnedProduct", b =>
                {
                    b.HasOne("AguasNico.Models.Cart", "Cart")
                        .WithMany("ReturnedProducts")
                        .HasForeignKey("CartID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");
                });

            modelBuilder.Entity("AguasNico.Models.Route", b =>
                {
                    b.HasOne("AguasNico.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("AguasNico.Models.Transfer", b =>
                {
                    b.HasOne("AguasNico.Models.Client", "Client")
                        .WithMany("Transfers")
                        .HasForeignKey("ClientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AguasNico.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AguasNico.Models.Cart", b =>
                {
                    b.Navigation("PaymentMethods");

                    b.Navigation("Products");

                    b.Navigation("ReturnedProducts");
                });

            modelBuilder.Entity("AguasNico.Models.Client", b =>
                {
                    b.Navigation("Carts");

                    b.Navigation("Products");

                    b.Navigation("Transfers");
                });

            modelBuilder.Entity("AguasNico.Models.PaymentMethod", b =>
                {
                    b.Navigation("Carts");
                });

            modelBuilder.Entity("AguasNico.Models.Route", b =>
                {
                    b.Navigation("Carts");

                    b.Navigation("DispatchedProducts");
                });
#pragma warning restore 612, 618
        }
    }
}
