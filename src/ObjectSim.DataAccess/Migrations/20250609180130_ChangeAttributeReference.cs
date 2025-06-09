using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObjectSim.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAttributeReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attributes_DataTypes_DataTypeIdId",
                table: "Attributes");

            migrationBuilder.RenameColumn(
                name: "DataTypeIdId",
                table: "Attributes",
                newName: "DataTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Attributes_DataTypeIdId",
                table: "Attributes",
                newName: "IX_Attributes_DataTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attributes_DataTypes_DataTypeId",
                table: "Attributes",
                column: "DataTypeId",
                principalTable: "DataTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.AddForeignKey(
                name: "FK_Attributes_DataTypes_DataTypeIdId",
                table: "Attributes",
                column: "DataTypeIdId",
                principalTable: "DataTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
