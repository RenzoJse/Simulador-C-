using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ObjectSim.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixingMigrationProblem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "DataTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "Id", "IsAbstract", "IsInterface", "IsSealed", "Name", "ParentId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), false, false, false, "Object", null });

            migrationBuilder.InsertData(
                table: "DataTypes",
                columns: new[] { "Id", "Discriminator", "MethodIds", "Name", "Type" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000005"), "ValueType", "[]", "void", "bool" });

            migrationBuilder.InsertData(
                table: "Methods",
                columns: new[] { "Id", "Abstract", "Accessibility", "ClassId", "IsOverride", "IsSealed", "MethodId", "Name", "TypeId" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000101"), false, 0, new Guid("00000000-0000-0000-0000-000000000001"), false, false, null, "Equals", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("00000000-0000-0000-0000-000000000102"), false, 0, new Guid("00000000-0000-0000-0000-000000000001"), false, false, null, "Equals (Object, Object)", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("00000000-0000-0000-0000-000000000103"), false, 2, new Guid("00000000-0000-0000-0000-000000000001"), false, false, null, "Finalize", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("00000000-0000-0000-0000-000000000104"), false, 0, new Guid("00000000-0000-0000-0000-000000000001"), false, false, null, "GetHashCode", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("00000000-0000-0000-0000-000000000105"), false, 0, new Guid("00000000-0000-0000-0000-000000000001"), false, false, null, "GetType", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("00000000-0000-0000-0000-000000000106"), false, 2, new Guid("00000000-0000-0000-0000-000000000001"), false, false, null, "MemberwiseClone", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("00000000-0000-0000-0000-000000000107"), false, 0, new Guid("00000000-0000-0000-0000-000000000001"), false, false, null, "ReferenceEquals", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("00000000-0000-0000-0000-000000000108"), false, 0, new Guid("00000000-0000-0000-0000-000000000001"), false, false, null, "ToString", new Guid("00000000-0000-0000-0000-000000000005") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Methods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000101"));

            migrationBuilder.DeleteData(
                table: "Methods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000102"));

            migrationBuilder.DeleteData(
                table: "Methods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000103"));

            migrationBuilder.DeleteData(
                table: "Methods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000104"));

            migrationBuilder.DeleteData(
                table: "Methods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000105"));

            migrationBuilder.DeleteData(
                table: "Methods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000106"));

            migrationBuilder.DeleteData(
                table: "Methods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000107"));

            migrationBuilder.DeleteData(
                table: "Methods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000108"));

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"));

            migrationBuilder.DropColumn(
                name: "Type",
                table: "DataTypes");
        }
    }
}
