using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class AppUserPlanPlanFix2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PlanName",
                table: "AppUserPlans",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PlanName",
                table: "AppUserPlans",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
