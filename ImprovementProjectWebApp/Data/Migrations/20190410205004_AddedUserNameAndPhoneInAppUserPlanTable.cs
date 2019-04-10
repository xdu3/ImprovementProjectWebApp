using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class AddedUserNameAndPhoneInAppUserPlanTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserPlans_AspNetUsers_ApplicationUserId",
                table: "AppUserPlans");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "AppUserPlans",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "AppUserPlans",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "AppUserPlans",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserPlans_AspNetUsers_ApplicationUserId",
                table: "AppUserPlans",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserPlans_AspNetUsers_ApplicationUserId",
                table: "AppUserPlans");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "AppUserPlans");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "AppUserPlans");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "AppUserPlans",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserPlans_AspNetUsers_ApplicationUserId",
                table: "AppUserPlans",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
