using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class checkinQfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckInQADetail_CheckInQuestion_IntroQuestionId",
                table: "CheckInQADetail");

            migrationBuilder.DropIndex(
                name: "IX_CheckInQADetail_IntroQuestionId",
                table: "CheckInQADetail");

            migrationBuilder.DropColumn(
                name: "IntroQuestionId",
                table: "CheckInQADetail");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckInQADetail_CheckInQuestion_CheckInQuestionId",
                table: "CheckInQADetail",
                column: "CheckInQuestionId",
                principalTable: "CheckInQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckInQADetail_CheckInQuestion_CheckInQuestionId",
                table: "CheckInQADetail");

            migrationBuilder.AddColumn<int>(
                name: "IntroQuestionId",
                table: "CheckInQADetail",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CheckInQADetail_IntroQuestionId",
                table: "CheckInQADetail",
                column: "IntroQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckInQADetail_CheckInQuestion_IntroQuestionId",
                table: "CheckInQADetail",
                column: "IntroQuestionId",
                principalTable: "CheckInQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
