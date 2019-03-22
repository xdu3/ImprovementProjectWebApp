using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class fix4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeekPlan_MealPlan_MealPlanId",
                table: "WeekPlan");

            migrationBuilder.DropIndex(
                name: "IX_WeekPlan_MealPlanId",
                table: "WeekPlan");

            migrationBuilder.DropColumn(
                name: "MealPlanId",
                table: "WeekPlan");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MealPlanId",
                table: "WeekPlan",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WeekPlan_MealPlanId",
                table: "WeekPlan",
                column: "MealPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeekPlan_MealPlan_MealPlanId",
                table: "WeekPlan",
                column: "MealPlanId",
                principalTable: "MealPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
