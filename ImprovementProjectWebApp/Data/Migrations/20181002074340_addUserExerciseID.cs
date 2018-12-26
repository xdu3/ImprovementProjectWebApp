using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class addUserExerciseID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutPlan_Exercise_ExerciseId",
                table: "WorkoutPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutPlan_AspNetUsers_UserId",
                table: "WorkoutPlan");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "WorkoutPlan",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ExerciseId",
                table: "WorkoutPlan",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutPlan_Exercise_ExerciseId",
                table: "WorkoutPlan",
                column: "ExerciseId",
                principalTable: "Exercise",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutPlan_AspNetUsers_UserId",
                table: "WorkoutPlan",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutPlan_Exercise_ExerciseId",
                table: "WorkoutPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutPlan_AspNetUsers_UserId",
                table: "WorkoutPlan");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "WorkoutPlan",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "ExerciseId",
                table: "WorkoutPlan",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutPlan_Exercise_ExerciseId",
                table: "WorkoutPlan",
                column: "ExerciseId",
                principalTable: "Exercise",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutPlan_AspNetUsers_UserId",
                table: "WorkoutPlan",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
