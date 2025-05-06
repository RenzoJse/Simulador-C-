using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ObjectSim.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddBuiltinValueTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"),
                column: "Type",
                value: "void");

            migrationBuilder.InsertData(
                table: "DataTypes",
                columns: new[] { "Id", "Discriminator", "MethodIds", "Name", "Type" },
                values: new object[,]
                {
                    { new Guid("1d9cd43c-e19b-4b24-ae0f-fb6cc43f1f27"), "ValueType", "[]", "decimal", "decimal" },
                    { new Guid("249d6656-0276-556c-a992-bcf6bfea8578"), "ValueType", "[]", "int", "int" },
                    { new Guid("49e4ea3e-e6d6-4eb7-a7de-01cf4dc1cf7a"), "ValueType", "[]", "char", "char" },
                    { new Guid("4e82822e-e6e1-44c1-9df9-7c43f7ecda5e"), "ValueType", "[]", "byte", "byte" },
                    { new Guid("729965ef-64e3-5607-939f-8e19784ef0e9"), "ValueType", "[]", "bool", "bool" },
                    { new Guid("75dfd62e-8d7c-48ee-9481-183ec3629936"), "ValueType", "[]", "float", "float" },
                    { new Guid("bd8e7c9e-e8d0-42f2-9479-63284c5c3fa0"), "ValueType", "[]", "double", "double" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("1d9cd43c-e19b-4b24-ae0f-fb6cc43f1f27"));

            migrationBuilder.DeleteData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("249d6656-0276-556c-a992-bcf6bfea8578"));

            migrationBuilder.DeleteData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("49e4ea3e-e6d6-4eb7-a7de-01cf4dc1cf7a"));

            migrationBuilder.DeleteData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("4e82822e-e6e1-44c1-9df9-7c43f7ecda5e"));

            migrationBuilder.DeleteData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("729965ef-64e3-5607-939f-8e19784ef0e9"));

            migrationBuilder.DeleteData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("75dfd62e-8d7c-48ee-9481-183ec3629936"));

            migrationBuilder.DeleteData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("bd8e7c9e-e8d0-42f2-9479-63284c5c3fa0"));

            migrationBuilder.UpdateData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"),
                column: "Type",
                value: "bool");
        }
    }
}
