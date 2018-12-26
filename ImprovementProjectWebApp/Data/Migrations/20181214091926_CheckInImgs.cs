using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class CheckInImgs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheckInImgs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BackImgURL = table.Column<string>(nullable: true),
                    CheckInQAId = table.Column<int>(nullable: false),
                    FrontImgURL = table.Column<string>(nullable: true),
                    LeftImgURL = table.Column<string>(nullable: true),
                    RightImgURL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckInImgs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckInImgs_CheckInQA_CheckInQAId",
                        column: x => x.CheckInQAId,
                        principalTable: "CheckInQA",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckInImgs_CheckInQAId",
                table: "CheckInImgs",
                column: "CheckInQAId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckInImgs");
        }
    }
}
