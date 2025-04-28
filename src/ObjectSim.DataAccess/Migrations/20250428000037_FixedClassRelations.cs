using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObjectSim.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixedClassRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ReferenceTypes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReferenceTypes",
                table: "ReferenceTypes",
                column: "Name");

            migrationBuilder.CreateTable(
                name: "ValueTypes",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValueTypes", x => x.Name);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attributes_Classes_Id",
                table: "Attributes");

            migrationBuilder.DropForeignKey(
                name: "FK_Methods_Classes_Id",
                table: "Methods");

            migrationBuilder.DropTable(
                name: "ValueTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReferenceTypes",
                table: "ReferenceTypes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ReferenceTypes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<Guid>(
                name: "ClassId",
                table: "Methods",
                type: "uniqueidentifier",
                nullable: true);

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
    }
}
