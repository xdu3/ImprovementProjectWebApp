using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class connectCheckIn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "CheckInQA",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CheckInQA_ApplicationUserId",
                table: "CheckInQA",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckInQA_AspNetUsers_ApplicationUserId",
                table: "CheckInQA",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckInQA_AspNetUsers_ApplicationUserId",
                table: "CheckInQA");

            migrationBuilder.DropIndex(
                name: "IX_CheckInQA_ApplicationUserId",
                table: "CheckInQA");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "CheckInQA");
        }
    }
}
