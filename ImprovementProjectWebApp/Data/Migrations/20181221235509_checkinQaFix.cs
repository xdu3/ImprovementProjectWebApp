using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class checkinQaFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckInQA_AspNetUsers_UserId",
                table: "CheckInQA");

            migrationBuilder.DropIndex(
                name: "IX_CheckInQA_UserId",
                table: "CheckInQA");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CheckInQA");

            migrationBuilder.AddColumn<int>(
                name: "AppUserPlanId",
                table: "CheckInQA",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CheckInQA_AppUserPlanId",
                table: "CheckInQA",
                column: "AppUserPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckInQA_AppUserPlans_AppUserPlanId",
                table: "CheckInQA",
                column: "AppUserPlanId",
                principalTable: "AppUserPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckInQA_AppUserPlans_AppUserPlanId",
                table: "CheckInQA");

            migrationBuilder.DropIndex(
                name: "IX_CheckInQA_AppUserPlanId",
                table: "CheckInQA");

            migrationBuilder.DropColumn(
                name: "AppUserPlanId",
                table: "CheckInQA");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CheckInQA",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CheckInQA_UserId",
                table: "CheckInQA",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckInQA_AspNetUsers_UserId",
                table: "CheckInQA",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
