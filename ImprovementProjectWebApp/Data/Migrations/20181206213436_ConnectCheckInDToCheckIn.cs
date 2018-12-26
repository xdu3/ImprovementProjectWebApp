using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class ConnectCheckInDToCheckIn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckInQADetail_CheckInQuestion_IntroQuestionId",
                table: "CheckInQADetail");

            migrationBuilder.AlterColumn<int>(
                name: "IntroQuestionId",
                table: "CheckInQADetail",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "CheckInQAId",
                table: "CheckInQADetail",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckInQADetail_CheckInQA_IntroQuestionId",
                table: "CheckInQADetail",
                column: "IntroQuestionId",
                principalTable: "CheckInQA",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckInQADetail_CheckInQuestion_IntroQuestionId",
                table: "CheckInQADetail",
                column: "IntroQuestionId",
                principalTable: "CheckInQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckInQADetail_CheckInQA_IntroQuestionId",
                table: "CheckInQADetail");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckInQADetail_CheckInQuestion_IntroQuestionId",
                table: "CheckInQADetail");

            migrationBuilder.DropColumn(
                name: "CheckInQAId",
                table: "CheckInQADetail");

            migrationBuilder.AlterColumn<int>(
                name: "IntroQuestionId",
                table: "CheckInQADetail",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckInQADetail_CheckInQuestion_IntroQuestionId",
                table: "CheckInQADetail",
                column: "IntroQuestionId",
                principalTable: "CheckInQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
