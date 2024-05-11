﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Finder.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Finder.Data.Migrations
{
    [DbContext(typeof(FinderContext))]
    [Migration("20240511174736_Fix")]
    partial class Fix
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Finder.Data.Entities.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("AddressLine1")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("AddressLine2")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone('utc')");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Addresses", "dbo");
                });

            modelBuilder.Entity("Finder.Data.Entities.ContactInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone('utc')");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Instagram")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("LinkedIn")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Other")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Skype")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Telegram")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("ContactInfos", "dbo");
                });

            modelBuilder.Entity("Finder.Data.Entities.NotificationSettings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("EnableNotifications")
                        .HasColumnType("boolean");

                    b.Property<bool>("EnableTagFiltration")
                        .HasColumnType("boolean");

                    b.Property<bool>("EnableTitleFiltration")
                        .HasColumnType("boolean");

                    b.Property<bool>("EnableUpdateNotifications")
                        .HasColumnType("boolean");

                    b.Property<List<string>>("FilterTags")
                        .HasColumnType("text[]");

                    b.Property<List<string>>("FilterTitles")
                        .HasColumnType("text[]");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("NotificationSettings");
                });

            modelBuilder.Entity("Finder.Data.Entities.PushSubscription", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Auth")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone('utc')");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Endpoint")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime?>("ExpirationTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("P256dh")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("PushSubscriptions", "dbo");
                });

            modelBuilder.Entity("Finder.Data.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<bool>("CanCreateHelpRequest")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("CanCreateRoles")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("CanDeleteRoles")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("CanDeleteUsers")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("CanEditRoles")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("CanEditUsers")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("CanMaintainSystem")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("CanRestoreUsers")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("CanSeeAllRoles")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("CanSeeAllUsers")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("CanSeeHelpRequests")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("CanSeeRoles")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("CanSeeUsers")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone('utc')");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsHidden")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Roles", "dbo");

                    b.HasData(
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000001"),
                            CanCreateHelpRequest = true,
                            CanCreateRoles = true,
                            CanDeleteRoles = true,
                            CanDeleteUsers = true,
                            CanEditRoles = true,
                            CanEditUsers = true,
                            CanMaintainSystem = true,
                            CanRestoreUsers = true,
                            CanSeeAllRoles = true,
                            CanSeeAllUsers = true,
                            CanSeeHelpRequests = true,
                            CanSeeRoles = true,
                            CanSeeUsers = true,
                            CreatedAt = new DateTime(2022, 11, 11, 1, 6, 0, 0, DateTimeKind.Utc),
                            IsHidden = true,
                            Name = "Admin",
                            Type = "Admin"
                        },
                        new
                        {
                            Id = new Guid("a0a80c03-abbc-eb11-cabb-0022480a1c0a"),
                            CanCreateHelpRequest = true,
                            CanCreateRoles = false,
                            CanDeleteRoles = false,
                            CanDeleteUsers = false,
                            CanEditRoles = false,
                            CanEditUsers = false,
                            CanMaintainSystem = false,
                            CanRestoreUsers = false,
                            CanSeeAllRoles = false,
                            CanSeeAllUsers = false,
                            CanSeeHelpRequests = false,
                            CanSeeRoles = false,
                            CanSeeUsers = false,
                            CreatedAt = new DateTime(2022, 11, 11, 1, 6, 0, 0, DateTimeKind.Utc),
                            IsHidden = true,
                            Name = "User",
                            Type = "User"
                        });
                });

            modelBuilder.Entity("Finder.Data.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone('utc')");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("FirstName")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("PasswordHash")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("RefreshToken")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime?>("RefreshTokenExpiresAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("RegistrationToken")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("RoleId")
                        .HasColumnType("uuid");

                    b.Property<string>("Status")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasDefaultValue("Pending");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("VerificationCode")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.ToTable("Users", "dbo");

                    b.HasData(
                        new
                        {
                            Id = new Guid("00a68a21-7b01-2211-abbc-0022480a1c03"),
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "roma.dan2001@gmail.com",
                            FirstName = "Roman",
                            LastName = "Danylevych",
                            PasswordHash = "2576c639ea2309626fee6232e624ba921afada44537b9fa6592f03d5a1da7dd375fbd17b2af56655323327e8fd75a46d4932d54c4df61595844bc95fd5979c56",
                            RoleId = new Guid("00000000-0000-0000-0000-000000000001"),
                            Status = "Active"
                        },
                        new
                        {
                            Id = new Guid("6ebd3c1c-4a07-4c53-8a71-493386f28261"),
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "nazariy.chetvertukha@gmail.com",
                            FirstName = "Nazariy",
                            LastName = "Chetvertukha",
                            PasswordHash = "2576c639ea2309626fee6232e624ba921afada44537b9fa6592f03d5a1da7dd375fbd17b2af56655323327e8fd75a46d4932d54c4df61595844bc95fd5979c56",
                            RoleId = new Guid("a0a80c03-abbc-eb11-cabb-0022480a1c0a"),
                            Status = "Active"
                        },
                        new
                        {
                            Id = new Guid("bafee45c-1874-4c40-b781-5221133b4c30"),
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "annstepaniuk12@gmail.com",
                            FirstName = "Ann",
                            LastName = "Stepaniuk",
                            PasswordHash = "2576c639ea2309626fee6232e624ba921afada44537b9fa6592f03d5a1da7dd375fbd17b2af56655323327e8fd75a46d4932d54c4df61595844bc95fd5979c56",
                            RoleId = new Guid("a0a80c03-abbc-eb11-cabb-0022480a1c0a"),
                            Status = "Active"
                        },
                        new
                        {
                            Id = new Guid("e823f508-ce4e-498f-a401-dbf40c892dbb"),
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "roman.sadokha01@gmail.com",
                            FirstName = "Roman",
                            LastName = "Sadokha",
                            PasswordHash = "2576c639ea2309626fee6232e624ba921afada44537b9fa6592f03d5a1da7dd375fbd17b2af56655323327e8fd75a46d4932d54c4df61595844bc95fd5979c56",
                            RoleId = new Guid("a0a80c03-abbc-eb11-cabb-0022480a1c0a"),
                            Status = "Active"
                        },
                        new
                        {
                            Id = new Guid("07afd050-0126-4610-8dae-854efa9fcfde"),
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "boredarthur@gmail.com",
                            FirstName = "Arthur",
                            LastName = "Zavolovych",
                            PasswordHash = "2576c639ea2309626fee6232e624ba921afada44537b9fa6592f03d5a1da7dd375fbd17b2af56655323327e8fd75a46d4932d54c4df61595844bc95fd5979c56",
                            RoleId = new Guid("a0a80c03-abbc-eb11-cabb-0022480a1c0a"),
                            Status = "Active"
                        });
                });

            modelBuilder.Entity("Finder.Data.Entities.UserDetails", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid?>("AddressId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ContactInfoId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone('utc')");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ImageThumbnailUrl")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("ContactInfoId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserDetails", "dbo");
                });

            modelBuilder.Entity("Finder.Data.Entities.NotificationSettings", b =>
                {
                    b.HasOne("Finder.Data.Entities.User", "User")
                        .WithOne("NotificationSettings")
                        .HasForeignKey("Finder.Data.Entities.NotificationSettings", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Finder.Data.Entities.PushSubscription", b =>
                {
                    b.HasOne("Finder.Data.Entities.User", "User")
                        .WithMany("PushSubscriptions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Finder.Data.Entities.User", b =>
                {
                    b.HasOne("Finder.Data.Entities.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Finder.Data.Entities.UserDetails", b =>
                {
                    b.HasOne("Finder.Data.Entities.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Finder.Data.Entities.ContactInfo", "ContactInfo")
                        .WithMany()
                        .HasForeignKey("ContactInfoId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Finder.Data.Entities.User", "User")
                        .WithOne("Details")
                        .HasForeignKey("Finder.Data.Entities.UserDetails", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("ContactInfo");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Finder.Data.Entities.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Finder.Data.Entities.User", b =>
                {
                    b.Navigation("Details");

                    b.Navigation("NotificationSettings");

                    b.Navigation("PushSubscriptions");
                });
#pragma warning restore 612, 618
        }
    }
}
