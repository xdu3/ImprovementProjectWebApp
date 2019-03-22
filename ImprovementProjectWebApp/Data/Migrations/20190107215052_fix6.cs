using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class fix6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckInQA_AppUserPlans_AppUserPlanId",
                table: "CheckInQA");

            migrationBuilder.RenameColumn(
                name: "AppUserPlanId",
                table: "CheckInQA",
                newName: "WeekPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_CheckInQA_AppUserPlanId",
                table: "CheckInQA",
                newName: "IX_CheckInQA_WeekPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckInQA_WeekPlan_WeekPlanId",
                table: "CheckInQA",
                column: "WeekPlanId",
                principalTable: "WeekPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckInQA_WeekPlan_WeekPlanId",
                table: "CheckInQA");

            migrationBuilder.RenameColumn(
                name: "WeekPlanId",
                table: "CheckInQA",
                newName: "AppUserPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_CheckInQA_WeekPlanId",
                table: "CheckInQA",
                newName: "IX_CheckInQA_AppUserPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckInQA_AppUserPlans_AppUserPlanId",
                table: "CheckInQA",
                column: "AppUserPlanId",
                principalTable: "AppUserPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
