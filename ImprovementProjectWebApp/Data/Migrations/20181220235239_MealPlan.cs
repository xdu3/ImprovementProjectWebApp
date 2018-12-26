using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class MealPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MealPlanId",
                table: "AppUserPlans",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MealPlan",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Default = table.Column<bool>(nullable: false),
                    URL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealPlan", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserPlans_MealPlanId",
                table: "AppUserPlans",
                column: "MealPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserPlans_MealPlan_MealPlanId",
                table: "AppUserPlans",
                column: "MealPlanId",
                principalTable: "MealPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserPlans_MealPlan_MealPlanId",
                table: "AppUserPlans");

            migrationBuilder.DropTable(
                name: "MealPlan");

            migrationBuilder.DropIndex(
                name: "IX_AppUserPlans_MealPlanId",
                table: "AppUserPlans");

            migrationBuilder.DropColumn(
                name: "MealPlanId",
                table: "AppUserPlans");
        }
    }
}
