﻿// <auto-generated />
using ImprovementProjectWebApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace ImprovementProjectWebApp.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20181024230017_addPlanTrackIdInWorkoutPlan")]
    partial class addPlanTrackIdInWorkoutPlan
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ImprovementProjectWebApp.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<int?>("CustomerProfileId");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("CustomerProfileId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.BodyPart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("BodyPart");
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.CheckInQA", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Answer")
                        .IsRequired();

                    b.Property<int>("CheckInQuestionId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("IntroQuestionId");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("IntroQuestionId");

                    b.HasIndex("UserId");

                    b.ToTable("CheckInQA");
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.CheckInQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Question")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("CheckInQuestion");
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.CustomerProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("Gender")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("PhoneNumber")
                        .IsRequired();

                    b.Property<string>("PlanEndDate")
                        .IsRequired();

                    b.Property<string>("PlanStartDate")
                        .IsRequired();

                    b.Property<string>("StartDate")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("CustomerProfile");
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.Exercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BodyPartId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("BodyPartId");

                    b.ToTable("Exercise");
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.IntroQA", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Answer")
                        .IsRequired();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("IntroQuestionId");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("IntroQuestionId");

                    b.HasIndex("UserId");

                    b.ToTable("IntroQA");
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.IntroQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Question")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("IntroQuestion");
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.Reps", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("WorkoutPlanId");

                    b.Property<int>("num");

                    b.HasKey("Id");

                    b.HasIndex("WorkoutPlanId");

                    b.ToTable("Reps");
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.WorkoutPlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Des");

                    b.Property<int>("ExerciseId");

                    b.Property<bool>("OtherTypeExercise");

                    b.Property<string>("PlanName")
                        .IsRequired();

                    b.Property<int>("PlanTrackId");

                    b.Property<int>("Sets");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("UserId");

                    b.ToTable("WorkoutPlan");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.ApplicationUser", b =>
                {
                    b.HasOne("ImprovementProjectWebApp.Models.CustomerProfile", "CustomerProfile")
                        .WithMany()
                        .HasForeignKey("CustomerProfileId");
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.CheckInQA", b =>
                {
                    b.HasOne("ImprovementProjectWebApp.Models.CheckInQuestion", "CheckInQuestion")
                        .WithMany()
                        .HasForeignKey("IntroQuestionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImprovementProjectWebApp.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.Exercise", b =>
                {
                    b.HasOne("ImprovementProjectWebApp.Models.BodyPart", "BodyPart")
                        .WithMany()
                        .HasForeignKey("BodyPartId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.IntroQA", b =>
                {
                    b.HasOne("ImprovementProjectWebApp.Models.IntroQuestion", "IntroQuestion")
                        .WithMany()
                        .HasForeignKey("IntroQuestionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImprovementProjectWebApp.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.Reps", b =>
                {
                    b.HasOne("ImprovementProjectWebApp.Models.WorkoutPlan", "WorkoutPlan")
                        .WithMany()
                        .HasForeignKey("WorkoutPlanId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.WorkoutPlan", b =>
                {
                    b.HasOne("ImprovementProjectWebApp.Models.Exercise", "Exercise")
                        .WithMany()
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImprovementProjectWebApp.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ImprovementProjectWebApp.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ImprovementProjectWebApp.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImprovementProjectWebApp.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ImprovementProjectWebApp.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
