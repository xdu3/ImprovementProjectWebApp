using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class UpdatedAppUserPlanTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "AppUserPlans");

            migrationBuilder.AddColumn<double>(
                name: "OrderTotal",
                table: "AppUserPlans",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "AppUserPlans",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "AppUserPlans",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "AppUserPlans",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderTotal",
                table: "AppUserPlans");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "AppUserPlans");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AppUserPlans");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "AppUserPlans");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "AppUserPlans",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
