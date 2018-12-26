using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class ConnectCheckInDToCheckInFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckInQADetail_CheckInQA_IntroQuestionId",
                table: "CheckInQADetail");

            migrationBuilder.CreateIndex(
                name: "IX_CheckInQADetail_CheckInQuestionId",
                table: "CheckInQADetail",
                column: "CheckInQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckInQADetail_CheckInQA_CheckInQuestionId",
                table: "CheckInQADetail",
                column: "CheckInQuestionId",
                principalTable: "CheckInQA",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckInQADetail_CheckInQA_CheckInQuestionId",
                table: "CheckInQADetail");

            migrationBuilder.DropIndex(
                name: "IX_CheckInQADetail_CheckInQuestionId",
                table: "CheckInQADetail");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckInQADetail_CheckInQA_IntroQuestionId",
                table: "CheckInQADetail",
                column: "IntroQuestionId",
                principalTable: "CheckInQA",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
