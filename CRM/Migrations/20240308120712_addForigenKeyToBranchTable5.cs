using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Migrations
{
    /// <inheritdoc />
    public partial class addForigenKeyToBranchTable5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Users_OrganizationIdFK",
                table: "Branches");

            migrationBuilder.RenameColumn(
                name: "OrganizationIdFK",
                table: "Branches",
                newName: "OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_Branches_OrganizationIdFK",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Users_OrganizationId",
                table: "Branches");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "Branches",
                newName: "OrganizationIdFK");

            migrationBuilder.RenameIndex(
                name: "IX_Branches_OrganizationId",
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
    }
}
