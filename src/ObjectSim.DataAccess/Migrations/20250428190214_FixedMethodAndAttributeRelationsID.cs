using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObjectSim.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixedMethodAndAttributeRelationsID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attributes_Classes_Id",
                table: "Attributes");

            migrationBuilder.DropForeignKey(
                name: "FK_Methods_Classes_Id",
                table: "Methods");

            migrationBuilder.AddColumn<Guid>(
                name: "ClassId",
                table: "Methods",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Methods_ClassId",
                table: "Methods",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_ClassId",
                table: "Attributes",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attributes_Classes_ClassId",
                table: "Attributes",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_Attributes_Classes_ClassId",
                table: "Attributes");

            migrationBuilder.DropForeignKey(
                name: "FK_Methods_Classes_ClassId",
                table: "Methods");

            migrationBuilder.DropIndex(
                name: "IX_Methods_ClassId",
                table: "Methods");

            migrationBuilder.DropIndex(
                name: "IX_Attributes_ClassId",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Methods");

            migrationBuilder.AddForeignKey(
                name: "FK_Attributes_Classes_Id",
                table: "Attributes",
                column: "Id",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
