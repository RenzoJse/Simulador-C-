using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObjectSim.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixingVariables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Variables_DataTypes_TypeId",
                table: "Variables");

            migrationBuilder.DropForeignKey(
                name: "FK_Variables_Methods_MethodId",
                table: "Variables");

            migrationBuilder.DropForeignKey(
                name: "FK_Variables_Methods_VariableId",
                table: "Variables");

            migrationBuilder.DropIndex(
                name: "IX_Variables_TypeId",
                table: "Variables");

            migrationBuilder.AlterColumn<Guid>(
                name: "MethodId",
                table: "Variables",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MethodId1",
                table: "Variables",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MethodId2",
                table: "Variables",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Variables_MethodId1",
                table: "Variables",
                column: "MethodId1");

            migrationBuilder.CreateIndex(
                name: "IX_Variables_MethodId2",
                table: "Variables",
                column: "MethodId2");

            migrationBuilder.AddForeignKey(
                name: "FK_Variables_Methods_MethodId",
                table: "Variables",
                column: "MethodId",
                principalTable: "Methods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Variables_Methods_MethodId1",
                table: "Variables",
                column: "MethodId1",
                principalTable: "Methods",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Variables_Methods_MethodId2",
                table: "Variables",
                column: "MethodId2",
                principalTable: "Methods",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Variables_Methods_MethodId",
                table: "Variables");

            migrationBuilder.DropForeignKey(
                name: "FK_Variables_Methods_MethodId1",
                table: "Variables");

            migrationBuilder.DropForeignKey(
                name: "FK_Variables_Methods_MethodId2",
                table: "Variables");

            migrationBuilder.DropIndex(
                name: "IX_Variables_MethodId1",
                table: "Variables");

            migrationBuilder.DropIndex(
                name: "IX_Variables_MethodId2",
                table: "Variables");

            migrationBuilder.DropColumn(
                name: "MethodId1",
                table: "Variables");

            migrationBuilder.DropColumn(
                name: "MethodId2",
                table: "Variables");

            migrationBuilder.AlterColumn<Guid>(
                name: "MethodId",
                table: "Variables",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Variables_TypeId",
                table: "Variables",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Variables_DataTypes_TypeId",
                table: "Variables",
                column: "TypeId",
                principalTable: "DataTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Variables_Methods_MethodId",
                table: "Variables",
                column: "MethodId",
                principalTable: "Methods",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Variables_Methods_VariableId",
                table: "Variables",
                column: "VariableId",
                principalTable: "Methods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
