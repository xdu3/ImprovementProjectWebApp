using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class DeleteConnetCPAndAppUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_CustomerProfile_CustomerProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "CustomerProfile");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CustomerProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CustomerProfileId",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerProfileId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerProfile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    Gender = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: false),
                    PlanEndDate = table.Column<string>(nullable: false),
                    PlanStartDate = table.Column<string>(nullable: false),
                    StartDate = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProfile", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CustomerProfileId",
                table: "AspNetUsers",
                column: "CustomerProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_CustomerProfile_CustomerProfileId",
                table: "AspNetUsers",
                column: "CustomerProfileId",
                principalTable: "CustomerProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
