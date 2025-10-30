using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObjectSim.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InvokeMethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvokeMethod_Methods_InvokeMethodId",
                table: "InvokeMethod");

            migrationBuilder.DropIndex(
                name: "IX_InvokeMethod_InvokeMethodId",
                table: "InvokeMethod");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_InvokeMethod_InvokeMethodId",
                table: "InvokeMethod",
                column: "InvokeMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvokeMethod_Methods_InvokeMethodId",
                table: "InvokeMethod",
                column: "InvokeMethodId",
                principalTable: "Methods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
