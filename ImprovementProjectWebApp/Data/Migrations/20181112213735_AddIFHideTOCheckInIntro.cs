using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class AddIFHideTOCheckInIntro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IfHide",
                table: "IntroQuestion",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IfHide",
                table: "CheckInQuestion",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IfHide",
                table: "IntroQuestion");

            migrationBuilder.DropColumn(
                name: "IfHide",
                table: "CheckInQuestion");
        }
    }
}
