using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObjectSim.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedEF : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MethodIds",
                table: "DataTypes");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "DataTypes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MethodIds",
                table: "DataTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DataTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"),
                columns: new[] { "MethodIds", "Name" },
                values: new object[] { "[]", "void" });

            migrationBuilder.UpdateData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("1d9cd43c-e19b-4b24-ae0f-fb6cc43f1f27"),
                columns: new[] { "MethodIds", "Name" },
                values: new object[] { "[]", "decimal" });

            migrationBuilder.UpdateData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("249d6656-0276-556c-a992-bcf6bfea8578"),
                columns: new[] { "MethodIds", "Name" },
                values: new object[] { "[]", "int" });

            migrationBuilder.UpdateData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("49e4ea3e-e6d6-4eb7-a7de-01cf4dc1cf7a"),
                columns: new[] { "MethodIds", "Name" },
                values: new object[] { "[]", "char" });

            migrationBuilder.UpdateData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("4e82822e-e6e1-44c1-9df9-7c43f7ecda5e"),
                columns: new[] { "MethodIds", "Name" },
                values: new object[] { "[]", "byte" });

            migrationBuilder.UpdateData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("729965ef-64e3-5607-939f-8e19784ef0e9"),
                columns: new[] { "MethodIds", "Name" },
                values: new object[] { "[]", "bool" });

            migrationBuilder.UpdateData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("75dfd62e-8d7c-48ee-9481-183ec3629936"),
                columns: new[] { "MethodIds", "Name" },
                values: new object[] { "[]", "float" });

            migrationBuilder.UpdateData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("bd8e7c9e-e8d0-42f2-9479-63284c5c3fa0"),
                columns: new[] { "MethodIds", "Name" },
                values: new object[] { "[]", "double" });
        }
    }
}
