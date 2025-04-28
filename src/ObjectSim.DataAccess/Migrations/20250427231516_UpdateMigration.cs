using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObjectSim.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocalVariable_Methods_MethodId",
                table: "LocalVariable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Attributes",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Attributes");

            migrationBuilder.DropForeignKey(
                name: "FK_Parameter_Methods_MethodId",
                table: "Parameter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Parameter",
                table: "Parameter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LocalVariable",
                table: "LocalVariable");

            migrationBuilder.DropColumn(
                name: "DataType",
                table: "Attributes");

            migrationBuilder.RenameTable(
                name: "Parameter",
                newName: "Parameters");

            migrationBuilder.RenameTable(
                name: "LocalVariable",
                newName: "LocalVariables");

            migrationBuilder.RenameIndex(
                name: "IX_Parameter_MethodId",
                table: "Parameters",
                newName: "IX_Parameters_MethodId");

            migrationBuilder.RenameIndex(
                name: "IX_LocalVariable_MethodId",
                table: "LocalVariables",
                newName: "IX_LocalVariables_MethodId");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Methods",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Accessibility",
                table: "Methods",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOverride",
                table: "Methods",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "MethodId",
                table: "Methods",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsInterface",
                table: "Classes",
                type: "bit",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ClassId",
                table: "Attributes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Attributes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.NewGuid());

            migrationBuilder.AddPrimaryKey(
                name: "PK_Attributes",
                table: "Attributes",
                column: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Parameters",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "LocalVariables",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Parameters",
                table: "Parameters",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocalVariables",
                table: "LocalVariables",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ReferenceTypes",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "IX_Methods_MethodId",
                table: "Methods",
                column: "MethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_LocalVariables_Methods_MethodId",
                table: "LocalVariables",
                column: "MethodId",
                principalTable: "Methods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Methods_Methods_MethodId",
                table: "Methods",
                column: "MethodId",
                principalTable: "Methods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Parameters_Methods_MethodId",
                table: "Parameters",
                column: "MethodId",
                principalTable: "Methods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocalVariables_Methods_MethodId",
                table: "LocalVariables");

            migrationBuilder.DropForeignKey(
                name: "FK_Methods_Methods_MethodId",
                table: "Methods");

            migrationBuilder.DropForeignKey(
                name: "FK_Parameters_Methods_MethodId",
                table: "Parameters");

            migrationBuilder.DropTable(
                name: "ReferenceTypes");

            migrationBuilder.DropIndex(
                name: "IX_Methods_MethodId",
                table: "Methods");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Parameters",
                table: "Parameters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LocalVariables",
                table: "LocalVariables");

            migrationBuilder.DropColumn(
                name: "IsOverride",
                table: "Methods");

            migrationBuilder.DropColumn(
                name: "MethodId",
                table: "Methods");

            migrationBuilder.DropColumn(
                name: "IsInterface",
                table: "Classes");

            migrationBuilder.RenameTable(
                name: "Parameters",
                newName: "Parameter");

            migrationBuilder.RenameTable(
                name: "LocalVariables",
                newName: "LocalVariable");

            migrationBuilder.RenameIndex(
                name: "IX_Parameters_MethodId",
                table: "Parameter",
                newName: "IX_Parameter_MethodId");

            migrationBuilder.RenameIndex(
                name: "IX_LocalVariables_MethodId",
                table: "LocalVariable",
                newName: "IX_LocalVariable_MethodId");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Methods",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Accessibility",
                table: "Methods",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClassId",
                table: "Attributes",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Attributes",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "DataType",
                table: "Attributes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Parameter",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "LocalVariable",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Parameter",
                table: "Parameter",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocalVariable",
                table: "LocalVariable",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LocalVariable_Methods_MethodId",
                table: "LocalVariable",
                column: "MethodId",
                principalTable: "Methods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parameter_Methods_MethodId",
                table: "Parameter",
                column: "MethodId",
                principalTable: "Methods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
