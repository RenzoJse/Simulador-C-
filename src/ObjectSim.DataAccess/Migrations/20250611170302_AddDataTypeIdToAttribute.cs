using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObjectSim.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddDataTypeIdToAttribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attributes_DataTypes_DataTypeId",
                table: "Attributes");

            migrationBuilder.DropIndex(
                name: "IX_Attributes_DataTypeId",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "DataTypeIdId",
                table: "Attributes");

            migrationBuilder.AddColumn<Guid>(
                name: "DataTypeId",
                table: "Attributes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.Empty);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DataTypeIdId",
                table: "Attributes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.Empty);

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_DataTypeId",
                table: "Attributes",
                column: "DataTypeIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attributes_DataTypes_DataTypeId",
                table: "Attributes",
                column: "DataTypeIdId",
                principalTable: "DataTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
