using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObjectSim.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReferenceTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ValueTypes",
                table: "ValueTypes");

            migrationBuilder.RenameTable(
                name: "ValueTypes",
                newName: "DataTypes");

            migrationBuilder.AddColumn<Guid>(
                name: "DataTypeIdId",
                table: "Attributes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "DataTypes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "DataTypes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "DataTypes",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DataTypes",
                table: "DataTypes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_DataTypeId",
                table: "Attributes",
                column: "DataTypeIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attributes_DataTypes_DataTypeId",
                table: "Attributes",
                column: "DataTypeIdId",
                principalTable: "DataTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attributes_DataTypes_DataTypeId",
                table: "Attributes");

            migrationBuilder.DropIndex(
                name: "IX_Attributes_DataTypeId",
                table: "Attributes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DataTypes",
                table: "DataTypes");

            migrationBuilder.DropColumn(
                name: "DataTypeIdId",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DataTypes");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "DataTypes");

            migrationBuilder.RenameTable(
                name: "DataTypes",
                newName: "ValueTypes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ValueTypes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ValueTypes",
                table: "ValueTypes",
                column: "Name");

            migrationBuilder.CreateTable(
                name: "ReferenceTypes",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferenceTypes", x => x.Name);
                });
        }
    }
}
