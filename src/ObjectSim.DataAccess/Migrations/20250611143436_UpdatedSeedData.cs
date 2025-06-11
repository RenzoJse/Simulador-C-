using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ObjectSim.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "Id", "IsAbstract", "IsInterface", "IsSealed", "Name", "NamespaceId", "ParentId" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000002"), false, false, true, "String", null, new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("00000000-0000-0000-0000-000000000003"), false, false, true, "Int32", null, new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("00000000-0000-0000-0000-000000000004"), false, false, true, "Boolean", null, new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("00000000-0000-0000-0000-000000000006"), false, false, true, "Char", null, new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("00000000-0000-0000-0000-000000000007"), false, false, true, "Decimal", null, new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("00000000-0000-0000-0000-000000000008"), false, false, true, "Byte", null, new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("00000000-0000-0000-0000-000000000009"), false, false, true, "float", null, new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("00000000-0000-0000-0000-000000000010"), false, false, true, "Double", null, new Guid("00000000-0000-0000-0000-000000000001") }
                });

            migrationBuilder.InsertData(
                table: "DataTypes",
                columns: new[] { "Id", "Discriminator", "Type" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000003"), "ValueType", "int" },
                    { new Guid("00000000-0000-0000-0000-000000000004"), "ValueType", "bool" },
                    { new Guid("00000000-0000-0000-0000-000000000006"), "ValueType", "char" },
                    { new Guid("00000000-0000-0000-0000-000000000007"), "ValueType", "decimal" },
                    { new Guid("00000000-0000-0000-0000-000000000008"), "ValueType", "byte" },
                    { new Guid("00000000-0000-0000-0000-000000000009"), "ValueType", "float" },
                    { new Guid("00000000-0000-0000-0000-000000000010"), "ValueType", "double" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"));

            migrationBuilder.InsertData(
                table: "DataTypes",
                columns: new[] { "Id", "Discriminator", "Type" },
                values: new object[,]
                {
                    { new Guid("1d9cd43c-e19b-4b24-ae0f-fb6cc43f1f27"), "ValueType", "decimal" },
                    { new Guid("249d6656-0276-556c-a992-bcf6bfea8578"), "ValueType", "int" },
                    { new Guid("49e4ea3e-e6d6-4eb7-a7de-01cf4dc1cf7a"), "ValueType", "char" },
                    { new Guid("4e82822e-e6e1-44c1-9df9-7c43f7ecda5e"), "ValueType", "byte" },
                    { new Guid("729965ef-64e3-5607-939f-8e19784ef0e9"), "ValueType", "bool" },
                    { new Guid("75dfd62e-8d7c-48ee-9481-183ec3629936"), "ValueType", "float" },
                    { new Guid("bd8e7c9e-e8d0-42f2-9479-63284c5c3fa0"), "ValueType", "double" }
                });
        }
    }
}
