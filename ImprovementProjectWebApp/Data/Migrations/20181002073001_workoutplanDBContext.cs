using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class workoutplanDBContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkoutPlan",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Des = table.Column<string>(nullable: true),
                    ExerciseId = table.Column<int>(nullable: true),
                    OtherTypeExercise = table.Column<bool>(nullable: false),
                    PlanName = table.Column<string>(nullable: false),
                    Reps = table.Column<int>(nullable: false),
                    Sets = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutPlan_Exercise_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkoutPlan_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutPlan_ExerciseId",
                table: "WorkoutPlan",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutPlan_UserId",
                table: "WorkoutPlan",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkoutPlan");
        }
    }
}
