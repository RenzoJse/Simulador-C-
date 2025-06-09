using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObjectSim.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AttributeDataTypeReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Namespaces_Namespaces_NamespaceId",
                table: "Namespaces");

            migrationBuilder.DropIndex(
                name: "IX_Namespaces_NamespaceId",
                table: "Namespaces");

            migrationBuilder.DropColumn(
                name: "NamespaceId",
                table: "Namespaces");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attributes_DataTypes_DataTypeId",
                table: "Attributes");

            migrationBuilder.RenameColumn(
                name: "DataTypeId",
                table: "Attributes",
                newName: "DataTypeIdId");

            migrationBuilder.RenameIndex(
                name: "IX_Attributes_DataTypeId",
                table: "Attributes",
                newName: "IX_Attributes_DataTypeIdId");

            migrationBuilder.AddColumn<Guid>(
                name: "NamespaceId",
                table: "Namespaces",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Namespaces_NamespaceId",
                table: "Namespaces",
                column: "NamespaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Namespaces_Namespaces_NamespaceId",
                table: "Namespaces",
                column: "NamespaceId",
                principalTable: "Namespaces",
                principalColumn: "Id");
        }
    }
}
