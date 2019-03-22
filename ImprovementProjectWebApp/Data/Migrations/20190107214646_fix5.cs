using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class fix5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WeekPlanId",
                table: "MealPlan",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MealPlan_WeekPlanId",
                table: "MealPlan",
                column: "WeekPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_MealPlan_WeekPlan_WeekPlanId",
                table: "MealPlan",
                column: "WeekPlanId",
                principalTable: "WeekPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealPlan_WeekPlan_WeekPlanId",
                table: "MealPlan");

            migrationBuilder.DropIndex(
                name: "IX_MealPlan_WeekPlanId",
                table: "MealPlan");

            migrationBuilder.DropColumn(
                name: "WeekPlanId",
                table: "MealPlan");
        }
    }
}
