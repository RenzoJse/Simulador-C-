using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObjectSim.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addAttribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Namespaces_NamespaceId",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_NamespaceId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "NamespaceId",
                table: "Classes");

            migrationBuilder.AddColumn<string>(
                name: "ClassIds",
                table: "Namespaces",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "ClassIdsSerialized",
                table: "Namespaces",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Attributes",
                columns: new[] { "Id", "ClassId", "DataTypeId", "IsStatic", "Name", "Visibility" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000201"), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000005"), false, "default", 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Attributes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000201"));

            migrationBuilder.DropColumn(
                name: "ClassIds",
                table: "Namespaces");

            migrationBuilder.DropColumn(
                name: "ClassIdsSerialized",
                table: "Namespaces");

            migrationBuilder.AddColumn<Guid>(
                name: "NamespaceId",
                table: "Classes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "NamespaceId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                column: "NamespaceId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                column: "NamespaceId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"),
                column: "NamespaceId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000006"),
                column: "NamespaceId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000007"),
                column: "NamespaceId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000008"),
                column: "NamespaceId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000009"),
                column: "NamespaceId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"),
                column: "NamespaceId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_NamespaceId",
                table: "Classes",
                column: "NamespaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Namespaces_NamespaceId",
                table: "Classes",
                column: "NamespaceId",
                principalTable: "Namespaces",
                principalColumn: "Id");
        }
    }
}
