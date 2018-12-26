using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class PlanFix1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutPlan_AspNetUsers_UserId",
                table: "WorkoutPlan");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutPlan_UserId",
                table: "WorkoutPlan");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WorkoutPlan");

            migrationBuilder.AddColumn<int>(
                name: "PlanId",
                table: "WorkoutPlan",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Plan",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    IfTemplate = table.Column<bool>(nullable: false),
                    PlanName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plan", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutPlan_PlanId",
                table: "WorkoutPlan",
                column: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutPlan_Plan_PlanId",
                table: "WorkoutPlan",
                column: "PlanId",
                principalTable: "Plan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutPlan_Plan_PlanId",
                table: "WorkoutPlan");

            migrationBuilder.DropTable(
                name: "Plan");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutPlan_PlanId",
                table: "WorkoutPlan");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "WorkoutPlan");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "WorkoutPlan",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutPlan_UserId",
                table: "WorkoutPlan",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutPlan_AspNetUsers_UserId",
                table: "WorkoutPlan",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
