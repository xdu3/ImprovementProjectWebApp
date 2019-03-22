using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class ExUrls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExURl",
                table: "Exercise",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExURl2",
                table: "Exercise",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExURl",
                table: "Exercise");

            migrationBuilder.DropColumn(
                name: "ExURl2",
                table: "Exercise");
        }
    }
}
