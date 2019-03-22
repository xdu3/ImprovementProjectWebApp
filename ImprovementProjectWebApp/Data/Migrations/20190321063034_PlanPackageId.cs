using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class PlanPackageId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "AppUserPlans",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "AppUserPlans",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlanPackageId",
                table: "AppUserPlans",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PlanPackage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Des = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    Term = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanPackage", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserPlans_PlanPackageId",
                table: "AppUserPlans",
                column: "PlanPackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserPlans_PlanPackage_PlanPackageId",
                table: "AppUserPlans",
                column: "PlanPackageId",
                principalTable: "PlanPackage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserPlans_PlanPackage_PlanPackageId",
                table: "AppUserPlans");

            migrationBuilder.DropTable(
                name: "PlanPackage");

            migrationBuilder.DropIndex(
                name: "IX_AppUserPlans_PlanPackageId",
                table: "AppUserPlans");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "AppUserPlans");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "AppUserPlans");

            migrationBuilder.DropColumn(
                name: "PlanPackageId",
                table: "AppUserPlans");
        }
    }
}
