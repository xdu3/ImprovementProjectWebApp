using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class CheckInDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckInQA_CheckInQuestion_IntroQuestionId",
                table: "CheckInQA");

            migrationBuilder.DropIndex(
                name: "IX_CheckInQA_IntroQuestionId",
                table: "CheckInQA");

            migrationBuilder.DropColumn(
                name: "Answer",
                table: "CheckInQA");

            migrationBuilder.DropColumn(
                name: "CheckInQuestionId",
                table: "CheckInQA");

            migrationBuilder.DropColumn(
                name: "IntroQuestionId",
                table: "CheckInQA");

            migrationBuilder.CreateTable(
                name: "CheckInQADetail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Answer = table.Column<string>(nullable: false),
                    CheckInQuestionId = table.Column<int>(nullable: false),
                    IntroQuestionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckInQADetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckInQADetail_CheckInQuestion_IntroQuestionId",
                        column: x => x.IntroQuestionId,
                        principalTable: "CheckInQuestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckInQADetail_IntroQuestionId",
                table: "CheckInQADetail",
                column: "IntroQuestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckInQADetail");

            migrationBuilder.AddColumn<string>(
                name: "Answer",
                table: "CheckInQA",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CheckInQuestionId",
                table: "CheckInQA",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IntroQuestionId",
                table: "CheckInQA",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CheckInQA_IntroQuestionId",
                table: "CheckInQA",
                column: "IntroQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckInQA_CheckInQuestion_IntroQuestionId",
                table: "CheckInQA",
                column: "IntroQuestionId",
                principalTable: "CheckInQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
