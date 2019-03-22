using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class fix2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppUserPlanId",
                table: "WeekPlan",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WeekPlan_AppUserPlanId",
                table: "WeekPlan",
                column: "AppUserPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeekPlan_AppUserPlans_AppUserPlanId",
                table: "WeekPlan",
                column: "AppUserPlanId",
                principalTable: "AppUserPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeekPlan_AppUserPlans_AppUserPlanId",
                table: "WeekPlan");

            migrationBuilder.DropIndex(
                name: "IX_WeekPlan_AppUserPlanId",
                table: "WeekPlan");

            migrationBuilder.DropColumn(
                name: "AppUserPlanId",
                table: "WeekPlan");
        }
    }
}
