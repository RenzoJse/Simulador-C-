using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ObjectSim.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddVoidTypeAndFixMethodSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Methods_Methods_MethodId",
                table: "Methods");

            migrationBuilder.DropTable(
                name: "LocalVariables");

            migrationBuilder.DropTable(
                name: "Parameters");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Methods");

            migrationBuilder.AddColumn<Guid>(
                name: "TypeId",
                table: "Methods",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "MethodIds",
                table: "DataTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "DataTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "DataTypeMethodLocalVariables",
                columns: table => new
                {
                    IdDataType = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdMethod = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataTypeMethodLocalVariables", x => new { x.IdDataType, x.IdMethod });
                    table.ForeignKey(
                        name: "FK_DataTypeMethodLocalVariables_DataTypes_IdDataType",
                        column: x => x.IdDataType,
                        principalTable: "DataTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DataTypeMethodLocalVariables_Methods_IdMethod",
                        column: x => x.IdMethod,
                        principalTable: "Methods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DataTypeMethodParameters",
                columns: table => new
                {
                    IdDataType = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdMethod = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataTypeMethodParameters", x => new { x.IdDataType, x.IdMethod });
                    table.ForeignKey(
                        name: "FK_DataTypeMethodParameters_DataTypes_IdDataType",
                        column: x => x.IdDataType,
                        principalTable: "DataTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DataTypeMethodParameters_Methods_IdMethod",
                        column: x => x.IdMethod,
                        principalTable: "Methods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Methods_TypeId",
                table: "Methods",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DataTypeMethodLocalVariables_IdMethod",
                table: "DataTypeMethodLocalVariables",
                column: "IdMethod");

            migrationBuilder.CreateIndex(
                name: "IX_DataTypeMethodParameters_IdMethod",
                table: "DataTypeMethodParameters",
                column: "IdMethod");

            migrationBuilder.AddForeignKey(
                name: "FK_Methods_DataTypes_TypeId",
                table: "Methods",
                column: "TypeId",
                principalTable: "DataTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Methods_Methods_MethodId",
                table: "Methods",
                column: "MethodId",
                principalTable: "Methods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Methods_DataTypes_TypeId",
                table: "Methods");

            migrationBuilder.DropForeignKey(
                name: "FK_Methods_Methods_MethodId",
                table: "Methods");

            migrationBuilder.DropTable(
                name: "DataTypeMethodLocalVariables");

            migrationBuilder.DropTable(
                name: "DataTypeMethodParameters");

            migrationBuilder.DropIndex(
                name: "IX_Methods_TypeId",
                table: "Methods");

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
                name: "TypeId",
                table: "Methods");

            migrationBuilder.DropColumn(
                name: "MethodIds",
                table: "DataTypes");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "DataTypes");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Methods",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "LocalVariables",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalVariables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocalVariables_Methods_MethodId",
                        column: x => x.MethodId,
                        principalTable: "Methods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parameters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parameters_Methods_MethodId",
                        column: x => x.MethodId,
                        principalTable: "Methods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocalVariables_MethodId",
                table: "LocalVariables",
                column: "MethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_MethodId",
                table: "Parameters",
                column: "MethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Methods_Methods_MethodId",
                table: "Methods",
                column: "MethodId",
                principalTable: "Methods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
