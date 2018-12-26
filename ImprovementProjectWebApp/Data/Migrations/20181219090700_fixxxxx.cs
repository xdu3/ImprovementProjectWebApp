using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class fixxxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserPlans_Plans_PlanId",
                table: "AppUserPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutPlan_Plans_PlanId",
                table: "WorkoutPlan");

            migrationBuilder.AlterColumn<int>(
                name: "PlanId",
                table: "WorkoutPlan",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PlanId",
                table: "AppUserPlans",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserPlans_Plans_PlanId",
                table: "AppUserPlans",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutPlan_Plans_PlanId",
                table: "WorkoutPlan",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserPlans_Plans_PlanId",
                table: "AppUserPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutPlan_Plans_PlanId",
                table: "WorkoutPlan");

            migrationBuilder.AlterColumn<int>(
                name: "PlanId",
                table: "WorkoutPlan",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "PlanId",
                table: "AppUserPlans",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserPlans_Plans_PlanId",
                table: "AppUserPlans",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutPlan_Plans_PlanId",
                table: "WorkoutPlan",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
