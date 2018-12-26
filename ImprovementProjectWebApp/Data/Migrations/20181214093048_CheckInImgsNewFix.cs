using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class CheckInImgsNewFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackImgURL",
                table: "CheckInImgs");

            migrationBuilder.DropColumn(
                name: "FrontImgURL",
                table: "CheckInImgs");

            migrationBuilder.DropColumn(
                name: "LeftImgURL",
                table: "CheckInImgs");

            migrationBuilder.RenameColumn(
                name: "RightImgURL",
                table: "CheckInImgs",
                newName: "ImgURL");

            migrationBuilder.AddColumn<int>(
                name: "ImgPart",
                table: "CheckInImgs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgPart",
                table: "CheckInImgs");

            migrationBuilder.RenameColumn(
                name: "ImgURL",
                table: "CheckInImgs",
                newName: "RightImgURL");

            migrationBuilder.AddColumn<string>(
                name: "BackImgURL",
                table: "CheckInImgs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FrontImgURL",
                table: "CheckInImgs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LeftImgURL",
                table: "CheckInImgs",
                nullable: true);
        }
    }
}
