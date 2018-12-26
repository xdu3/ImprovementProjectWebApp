using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class workoutPlanFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "WorkoutPlan");

            migrationBuilder.DropColumn(
                name: "PlanName",
                table: "WorkoutPlan");

            migrationBuilder.DropColumn(
                name: "PlanTrackId",
                table: "WorkoutPlan");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "WorkoutPlan",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PlanName",
                table: "WorkoutPlan",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PlanTrackId",
                table: "WorkoutPlan",
                nullable: false,
                defaultValue: 0);
        }
    }
}
