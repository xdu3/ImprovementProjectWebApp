using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class AppUserPlanPlanFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutPlan_Plan_PlanId",
                table: "WorkoutPlan");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Plan",
                table: "Plan");

            migrationBuilder.RenameTable(
                name: "Plan",
                newName: "Plans");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Plans",
                table: "Plans",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AppUserPlans",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: false),
                    IfLock = table.Column<bool>(nullable: false),
                    PlanId = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUserPlans_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppUserPlans_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserPlans_ApplicationUserId",
                table: "AppUserPlans",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserPlans_PlanId",
                table: "AppUserPlans",
                column: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutPlan_Plans_PlanId",
                table: "WorkoutPlan",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutPlan_Plans_PlanId",
                table: "WorkoutPlan");

            migrationBuilder.DropTable(
                name: "AppUserPlans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Plans",
                table: "Plans");

            migrationBuilder.RenameTable(
                name: "Plans",
                newName: "Plan");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Plan",
                table: "Plan",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutPlan_Plan_PlanId",
                table: "WorkoutPlan",
                column: "PlanId",
                principalTable: "Plan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
