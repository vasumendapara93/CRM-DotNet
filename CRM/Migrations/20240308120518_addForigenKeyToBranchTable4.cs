using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Migrations
{
    /// <inheritdoc />
    public partial class addForigenKeyToBranchTable4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Users_OrganizationIdFk",
                table: "Branches");

            migrationBuilder.RenameColumn(
                name: "OrganizationIdFk",
                table: "Branches",
                newName: "OrganizationIdFK");

            migrationBuilder.RenameIndex(
                name: "IX_Branches_OrganizationIdFk",
                table: "Branches",
                newName: "IX_Branches_OrganizationIdFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_Users_OrganizationIdFK",
                table: "Branches",
                column: "OrganizationIdFK",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Users_OrganizationIdFK",
                table: "Branches");

            migrationBuilder.RenameColumn(
                name: "OrganizationIdFK",
                table: "Branches",
                newName: "OrganizationIdFk");

            migrationBuilder.RenameIndex(
                name: "IX_Branches_OrganizationIdFK",
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
    }
}
