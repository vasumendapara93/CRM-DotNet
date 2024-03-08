using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Migrations
{
    /// <inheritdoc />
    public partial class addForigenKeyToBranchTable3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Users_OrganizationId",
                table: "Branches");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "Branches",
                newName: "OrganizationIdFk");

            migrationBuilder.RenameIndex(
                name: "IX_Branches_OrganizationId",
                table: "Branches",
                newName: "IX_Branches_OrganizationIdFk");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_Users_OrganizationIdFk",
                table: "Branches",
                column: "OrganizationIdFk",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Users_OrganizationIdFk",
                table: "Branches");

            migrationBuilder.RenameColumn(
                name: "OrganizationIdFk",
                table: "Branches",
                newName: "OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_Branches_OrganizationIdFk",
                table: "Branches",
                newName: "IX_Branches_OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_Users_OrganizationId",
                table: "Branches",
                column: "OrganizationId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
