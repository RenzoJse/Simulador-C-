using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObjectSim.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InvokeMethodTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Methods_Methods_MethodId",
                table: "Methods");

            migrationBuilder.DropIndex(
                name: "IX_Methods_MethodId",
                table: "Methods");

            migrationBuilder.DropColumn(
                name: "MethodId",
                table: "Methods");

            migrationBuilder.CreateTable(
                name: "InvokeMethod",
                columns: table => new
                {
                    MethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvokeMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvokeMethod", x => new { x.MethodId, x.InvokeMethodId });
                    table.ForeignKey(
                        name: "FK_InvokeMethod_Methods_InvokeMethodId",
                        column: x => x.InvokeMethodId,
                        principalTable: "Methods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvokeMethod_Methods_MethodId",
                        column: x => x.MethodId,
                        principalTable: "Methods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvokeMethod_InvokeMethodId",
                table: "InvokeMethod",
                column: "InvokeMethodId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvokeMethod");

            migrationBuilder.AddColumn<Guid>(
                name: "MethodId",
                table: "Methods",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Methods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000101"),
                column: "MethodId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Methods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000102"),
                column: "MethodId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Methods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000103"),
                column: "MethodId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Methods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000104"),
                column: "MethodId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Methods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000105"),
                column: "MethodId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Methods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000106"),
                column: "MethodId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Methods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000107"),
                column: "MethodId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Methods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000108"),
                column: "MethodId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Methods_MethodId",
                table: "Methods",
                column: "MethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Methods_Methods_MethodId",
                table: "Methods",
                column: "MethodId",
                principalTable: "Methods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
