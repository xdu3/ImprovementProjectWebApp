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
    [Migration("20190320035801_addBirthday")]
    partial class addBirthday
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

                    b.Property<DateTime>("Birthday");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("IfDelete");

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

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.AppUserPlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserId");

                    b.Property<DateTime>("EndDate");

                    b.Property<bool>("IfLock");

                    b.Property<decimal>("Price");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("AppUserPlans");
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

            modelBuilder.Entity("ImprovementProjectWebApp.Models.CheckInImgs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CheckInQAId");

                    b.Property<int>("ImgPart");

                    b.Property<string>("ImgURL");

                    b.HasKey("Id");

                    b.HasIndex("CheckInQAId");

                    b.ToTable("CheckInImgs");
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.CheckInQA", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("ApplicationUserId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("EndDate");

                    b.Property<bool>("IntroCheckInQA");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("CheckInQA");
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.CheckInQADetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Answer");

                    b.Property<int>("CheckInQAId");

                    b.Property<int>("CheckInQuestionId");

                    b.HasKey("Id");

                    b.HasIndex("CheckInQAId");

                    b.HasIndex("CheckInQuestionId");

                    b.ToTable("CheckInQADetail");
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.CheckInQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IfHide");

                    b.Property<string>("Question")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("CheckInQuestion");
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.CustomerProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserId");

                    b.Property<string>("Gender")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("PhoneNumber")
                        .IsRequired();

                    b.Property<DateTime>("StartDate");

                    b.Property<string>("WeChatNumber")
                        .IsRequired();

                    b.Property<byte[]>("WeChatQRCode");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("CustomerProfile");
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.Exercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BodyPartId");

                    b.Property<string>("ExURl");

                    b.Property<string>("ExURl2");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("BodyPartId");

                    b.ToTable("Exercise");
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.FeedBack", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Answer");

                    b.Property<string>("ApplicationUserId");

                    b.Property<string>("Qustion");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("FeedBack");
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

                    b.Property<bool>("IfHide");

                    b.Property<string>("Question")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("IntroQuestion");
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.MealPlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Default");

                    b.Property<string>("URL");

                    b.Property<int>("WeekPlanId");

                    b.HasKey("Id");

                    b.HasIndex("WeekPlanId");

                    b.ToTable("MealPlan");
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.Plan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DayPlanDate");

                    b.Property<int>("DayPlanNum");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("WeekPlanId");

                    b.HasKey("Id");

                    b.HasIndex("WeekPlanId");

                    b.ToTable("Plans");
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

            modelBuilder.Entity("ImprovementProjectWebApp.Models.UserCheckInDate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AppUserPlanId");

                    b.Property<string>("ApplicationUserId");

                    b.Property<DateTime>("CheckInDate");

                    b.HasKey("Id");

                    b.HasIndex("AppUserPlanId");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("UserCheckInDate");
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.WeekPlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AppUserPlanId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<DateTime>("WeekPlanEndTime");

                    b.Property<DateTime>("WeekPlanStartTime");

                    b.HasKey("Id");

                    b.HasIndex("AppUserPlanId");

                    b.ToTable("WeekPlan");
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.WorkoutPlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Des");

                    b.Property<int>("ExerciseId");

                    b.Property<bool>("OtherTypeExercise");

                    b.Property<int>("PlanId");

                    b.Property<bool>("ProgressiveOverload");

                    b.Property<int>("Sets");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("PlanId");

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

            modelBuilder.Entity("ImprovementProjectWebApp.Models.AppUserPlan", b =>
                {
                    b.HasOne("ImprovementProjectWebApp.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("AppUserPlans")
                        .HasForeignKey("ApplicationUserId");
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.CheckInImgs", b =>
                {
                    b.HasOne("ImprovementProjectWebApp.Models.CheckInQA", "CheckInQA")
                        .WithMany("CheckInImgs")
                        .HasForeignKey("CheckInQAId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.CheckInQA", b =>
                {
                    b.HasOne("ImprovementProjectWebApp.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("CheckInQAs")
                        .HasForeignKey("ApplicationUserId");
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.CheckInQADetail", b =>
                {
                    b.HasOne("ImprovementProjectWebApp.Models.CheckInQA", "CheckInQA")
                        .WithMany("CheckInQADetails")
                        .HasForeignKey("CheckInQAId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImprovementProjectWebApp.Models.CheckInQuestion", "CheckInQuestion")
                        .WithMany()
                        .HasForeignKey("CheckInQuestionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.CustomerProfile", b =>
                {
                    b.HasOne("ImprovementProjectWebApp.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("CustomerProfiles")
                        .HasForeignKey("ApplicationUserId");
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.Exercise", b =>
                {
                    b.HasOne("ImprovementProjectWebApp.Models.BodyPart", "BodyPart")
                        .WithMany()
                        .HasForeignKey("BodyPartId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.FeedBack", b =>
                {
                    b.HasOne("ImprovementProjectWebApp.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("FeedBacks")
                        .HasForeignKey("ApplicationUserId");
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.IntroQA", b =>
                {
                    b.HasOne("ImprovementProjectWebApp.Models.IntroQuestion", "IntroQuestion")
                        .WithMany()
                        .HasForeignKey("IntroQuestionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImprovementProjectWebApp.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("IntroQAs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.MealPlan", b =>
                {
                    b.HasOne("ImprovementProjectWebApp.Models.WeekPlan", "WeekPlan")
                        .WithMany("MealPlan")
                        .HasForeignKey("WeekPlanId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.Plan", b =>
                {
                    b.HasOne("ImprovementProjectWebApp.Models.WeekPlan", "WeekPlan")
                        .WithMany("Plans")
                        .HasForeignKey("WeekPlanId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.Reps", b =>
                {
                    b.HasOne("ImprovementProjectWebApp.Models.WorkoutPlan", "WorkoutPlan")
                        .WithMany("Reps")
                        .HasForeignKey("WorkoutPlanId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.UserCheckInDate", b =>
                {
                    b.HasOne("ImprovementProjectWebApp.Models.AppUserPlan", "AppUserPlan")
                        .WithMany()
                        .HasForeignKey("AppUserPlanId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImprovementProjectWebApp.Models.ApplicationUser")
                        .WithMany("UserCheckInDates")
                        .HasForeignKey("ApplicationUserId");
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.WeekPlan", b =>
                {
                    b.HasOne("ImprovementProjectWebApp.Models.AppUserPlan", "AppUserPlan")
                        .WithMany("WeekPlans")
                        .HasForeignKey("AppUserPlanId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImprovementProjectWebApp.Models.WorkoutPlan", b =>
                {
                    b.HasOne("ImprovementProjectWebApp.Models.Exercise", "Exercise")
                        .WithMany()
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImprovementProjectWebApp.Models.Plan", "Plan")
                        .WithMany("WorkoutPlans")
                        .HasForeignKey("PlanId")
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