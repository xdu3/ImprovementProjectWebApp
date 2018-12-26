using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class repstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reps",
                table: "WorkoutPlan");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Reps",
                table: "WorkoutPlan",
                nullable: false,
                defaultValue: 0);
        }
    }
}
