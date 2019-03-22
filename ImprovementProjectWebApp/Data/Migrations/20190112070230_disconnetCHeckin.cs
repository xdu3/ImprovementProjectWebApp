using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class disconnetCHeckin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckInQA_WeekPlan_WeekPlanId",
                table: "CheckInQA");

            migrationBuilder.DropIndex(
                name: "IX_CheckInQA_WeekPlanId",
                table: "CheckInQA");

            migrationBuilder.DropColumn(
                name: "WeekPlanId",
                table: "CheckInQA");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WeekPlanId",
                table: "CheckInQA",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CheckInQA_WeekPlanId",
                table: "CheckInQA",
                column: "WeekPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckInQA_WeekPlan_WeekPlanId",
                table: "CheckInQA",
                column: "WeekPlanId",
                principalTable: "WeekPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
