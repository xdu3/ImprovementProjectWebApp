using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class register : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "CustomerProfile");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "CustomerProfile",
                nullable: false,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StartDate",
                table: "CustomerProfile",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "CustomerProfile",
                nullable: false,
                defaultValue: "");
        }
    }
}
