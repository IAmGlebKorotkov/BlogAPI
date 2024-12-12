﻿// <auto-generated />
using System;
using BlogAPI.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BlogAPI.Migrations.Community
{
    [DbContext(typeof(CommunityContext))]
    [Migration("20241210185418_RenameAdminCommunityToUserCommunity")]
    partial class RenameAdminCommunityToUserCommunity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BlogAPI.Models.Community.CommunityFullDto", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("create_time");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("boolean")
                        .HasColumnName("is_closed");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("SubscribersCount")
                        .HasColumnType("integer")
                        .HasColumnName("subscribers_count");

                    b.HasKey("Id");

                    b.ToTable("community", "fias");
                });

            modelBuilder.Entity("BlogAPI.Models.Community.CommunityUserDto", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.Property<string>("CommunityId")
                        .HasColumnType("text")
                        .HasColumnName("community_id");

                    b.Property<int>("Role")
                        .HasColumnType("integer")
                        .HasColumnName("role");

                    b.HasKey("UserId", "CommunityId");

                    b.HasIndex("CommunityId");

                    b.ToTable("community_users", "fias");
                });

            modelBuilder.Entity("BlogAPI.Models.Community.CommunityUserDto", b =>
                {
                    b.HasOne("BlogAPI.Models.Community.CommunityFullDto", null)
                        .WithMany("Administrators")
                        .HasForeignKey("CommunityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BlogAPI.Models.Community.CommunityFullDto", b =>
                {
                    b.Navigation("Administrators");
                });
#pragma warning restore 612, 618
        }
    }
}
