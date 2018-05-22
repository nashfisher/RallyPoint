﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Rallypoint.Models;
using System;

namespace Rallypoint.Migrations
{
    [DbContext(typeof(RallypointContext))]
    [Migration("20180522160615_FirstMigration")]
    partial class FirstMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011");

            modelBuilder.Entity("Rallypoint.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("address");

                    b.Property<DateTime>("date");

                    b.Property<int?>("playeroneId");

                    b.Property<int?>("playeroneScore");

                    b.Property<int?>("playertwoId");

                    b.Property<int?>("playertwoScore");

                    b.HasKey("Id");

                    b.HasIndex("playeroneId");

                    b.HasIndex("playertwoId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Rallypoint.Models.Like", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("PostId");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("Rallypoint.Models.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("UserId");

                    b.Property<int?>("likes");

                    b.Property<string>("post");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Rallypoint.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("admin");

                    b.Property<string>("email");

                    b.Property<string>("first_name");

                    b.Property<string>("last_name");

                    b.Property<int>("losses");

                    b.Property<string>("password");

                    b.Property<string>("username");

                    b.Property<int>("wins");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Rallypoint.Models.Game", b =>
                {
                    b.HasOne("Rallypoint.Models.User", "playerone")
                        .WithMany("gamescreated")
                        .HasForeignKey("playeroneId");

                    b.HasOne("Rallypoint.Models.User", "playertwo")
                        .WithMany("gamesjoined")
                        .HasForeignKey("playertwoId");
                });

            modelBuilder.Entity("Rallypoint.Models.Like", b =>
                {
                    b.HasOne("Rallypoint.Models.Post", "post")
                        .WithMany()
                        .HasForeignKey("PostId");

                    b.HasOne("Rallypoint.Models.User", "user")
                        .WithMany("likedpost")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Rallypoint.Models.Post", b =>
                {
                    b.HasOne("Rallypoint.Models.User", "user")
                        .WithMany("posts")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
