using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class dontKnowWhatHappen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "UserCheckInDate",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCheckInDate_ApplicationUserId",
                table: "UserCheckInDate",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCheckInDate_AspNetUsers_ApplicationUserId",
                table: "UserCheckInDate",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCheckInDate_AspNetUsers_ApplicationUserId",
                table: "UserCheckInDate");

            migrationBuilder.DropIndex(
                name: "IX_UserCheckInDate_ApplicationUserId",
                table: "UserCheckInDate");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "UserCheckInDate");
        }
    }
}
