using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObjectSim.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixMethodClassRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Methods_Classes_Id",
                table: "Methods");

            migrationBuilder.AddColumn<Guid>(
                name: "ClassId",
                table: "Methods",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Methods_ClassId",
                table: "Methods",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Methods_Classes_ClassId",
                table: "Methods",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Methods_Classes_ClassId",
                table: "Methods");

            migrationBuilder.DropIndex(
                name: "IX_Methods_ClassId",
                table: "Methods");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Methods");

            migrationBuilder.AddForeignKey(
                name: "FK_Methods_Classes_Id",
                table: "Methods",
                column: "Id",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
