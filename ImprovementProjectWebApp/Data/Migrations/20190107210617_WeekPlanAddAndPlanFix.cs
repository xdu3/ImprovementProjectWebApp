using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class WeekPlanAddAndPlanFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "IfTemplate",
                table: "Plans");

            migrationBuilder.AddColumn<DateTime>(
                name: "DayPlanDate",
                table: "Plans",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "WeekPlanId",
                table: "Plans",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "WeekPlan",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    WeekPlanEndTime = table.Column<DateTime>(nullable: false),
                    WeekPlanStartTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeekPlan", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Plans_WeekPlanId",
                table: "Plans",
                column: "WeekPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_WeekPlan_WeekPlanId",
                table: "Plans",
                column: "WeekPlanId",
                principalTable: "WeekPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plans_WeekPlan_WeekPlanId",
                table: "Plans");

            migrationBuilder.DropTable(
                name: "WeekPlan");

            migrationBuilder.DropIndex(
                name: "IX_Plans_WeekPlanId",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "DayPlanDate",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "WeekPlanId",
                table: "Plans");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Plans",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IfTemplate",
                table: "Plans",
                nullable: false,
                defaultValue: false);
        }
    }
}
