using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class CheckinQaDfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckInQADetail_CheckInQA_CheckInQuestionId",
                table: "CheckInQADetail");

            migrationBuilder.CreateIndex(
                name: "IX_CheckInQADetail_CheckInQAId",
                table: "CheckInQADetail",
                column: "CheckInQAId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckInQADetail_CheckInQA_CheckInQAId",
                table: "CheckInQADetail",
                column: "CheckInQAId",
                principalTable: "CheckInQA",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckInQADetail_CheckInQA_CheckInQAId",
                table: "CheckInQADetail");

            migrationBuilder.DropIndex(
                name: "IX_CheckInQADetail_CheckInQAId",
                table: "CheckInQADetail");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckInQADetail_CheckInQA_CheckInQuestionId",
                table: "CheckInQADetail",
                column: "CheckInQuestionId",
                principalTable: "CheckInQA",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
