using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class fix1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserPlans_Plans_PlanId",
                table: "AppUserPlans");

            migrationBuilder.DropIndex(
                name: "IX_AppUserPlans_PlanId",
                table: "AppUserPlans");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "AppUserPlans");

            migrationBuilder.RenameColumn(
                name: "PlanName",
                table: "Plans",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Plans",
                newName: "PlanName");

            migrationBuilder.AddColumn<int>(
                name: "PlanId",
                table: "AppUserPlans",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AppUserPlans_PlanId",
                table: "AppUserPlans",
                column: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserPlans_Plans_PlanId",
                table: "AppUserPlans",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
