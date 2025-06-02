using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObjectSim.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedValidKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "NamespaceId",
                table: "Classes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Keys",
                columns: table => new
                {
                    AccessKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keys", x => x.AccessKey);
                });

            migrationBuilder.CreateTable(
                name: "Namespaces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NamespaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Namespaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Namespaces_Namespaces_NamespaceId",
                        column: x => x.NamespaceId,
                        principalTable: "Namespaces",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Namespaces_Namespaces_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Namespaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "NamespaceId",
                value: null);

            migrationBuilder.InsertData(
                table: "Keys",
                column: "AccessKey",
                values: new object[]
                {
                    new Guid("515dd649-30a0-4d57-9302-62a8db8179bd"),
                    new Guid("9c0ff0b1-4abd-45c6-8a4a-831748fb7a20")
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classes_NamespaceId",
                table: "Classes",
                column: "NamespaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Namespaces_NamespaceId",
                table: "Namespaces",
                column: "NamespaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Namespaces_ParentId",
                table: "Namespaces",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Namespaces_NamespaceId",
                table: "Classes",
                column: "NamespaceId",
                principalTable: "Namespaces",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Namespaces_NamespaceId",
                table: "Classes");

            migrationBuilder.DropTable(
                name: "Keys");

            migrationBuilder.DropTable(
                name: "Namespaces");

            migrationBuilder.DropIndex(
                name: "IX_Classes_NamespaceId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "NamespaceId",
                table: "Classes");
        }
    }
}
