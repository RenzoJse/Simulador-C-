using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObjectSim.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedVariables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataTypeMethodLocalVariables");

            migrationBuilder.DropTable(
                name: "DataTypeMethodParameters");

            migrationBuilder.CreateTable(
                name: "Variables",
                columns: table => new
                {
                    VariableId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Variables", x => x.VariableId);
                    table.ForeignKey(
                        name: "FK_Variables_DataTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "DataTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Variables_Methods_MethodId",
                        column: x => x.MethodId,
                        principalTable: "Methods",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Variables_Methods_VariableId",
                        column: x => x.VariableId,
                        principalTable: "Methods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                column: "Type",
                value: "int");

            migrationBuilder.UpdateData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"),
                column: "Type",
                value: "bool");

            migrationBuilder.UpdateData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"),
                column: "Type",
                value: "void");

            migrationBuilder.UpdateData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000006"),
                column: "Type",
                value: "char");

            migrationBuilder.UpdateData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000007"),
                column: "Type",
                value: "decimal");

            migrationBuilder.UpdateData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000008"),
                column: "Type",
                value: "byte");

            migrationBuilder.UpdateData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000009"),
                column: "Type",
                value: "float");

            migrationBuilder.UpdateData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"),
                column: "Type",
                value: "double");

            migrationBuilder.CreateIndex(
                name: "IX_Variables_MethodId",
                table: "Variables",
                column: "MethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Variables_TypeId",
                table: "Variables",
                column: "TypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Variables");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "DataTypes",
                newName: "Name");

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

            migrationBuilder.UpdateData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                column: "Name",
                value: null);

            migrationBuilder.UpdateData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"),
                column: "Name",
                value: null);

            migrationBuilder.UpdateData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"),
                column: "Name",
                value: null);

            migrationBuilder.UpdateData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000006"),
                column: "Name",
                value: null);

            migrationBuilder.UpdateData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000007"),
                column: "Name",
                value: null);

            migrationBuilder.UpdateData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000008"),
                column: "Name",
                value: null);

            migrationBuilder.UpdateData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000009"),
                column: "Name",
                value: null);

            migrationBuilder.UpdateData(
                table: "DataTypes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"),
                column: "Name",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_DataTypeMethodLocalVariables_IdMethod",
                table: "DataTypeMethodLocalVariables",
                column: "IdMethod");

            migrationBuilder.CreateIndex(
                name: "IX_DataTypeMethodParameters_IdMethod",
                table: "DataTypeMethodParameters",
                column: "IdMethod");
        }
    }
}
