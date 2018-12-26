using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImprovementProjectWebApp.Data.Migrations
{
    public partial class addBodtyPartIdInExercise : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercise_BodyPart_BodyPartId",
                table: "Exercise");

            migrationBuilder.AlterColumn<int>(
                name: "BodyPartId",
                table: "Exercise",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Exercise_BodyPart_BodyPartId",
                table: "Exercise",
                column: "BodyPartId",
                principalTable: "BodyPart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercise_BodyPart_BodyPartId",
                table: "Exercise");

            migrationBuilder.AlterColumn<int>(
                name: "BodyPartId",
                table: "Exercise",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Exercise_BodyPart_BodyPartId",
                table: "Exercise",
                column: "BodyPartId",
                principalTable: "BodyPart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
