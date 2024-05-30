using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Migrations
{
    /// <inheritdoc />
    public partial class AssigerAndSalesPersonInLeadsTableForigenKeyAppliedWithUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Assigner",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "SellsPerson",
                table: "Leads");

            migrationBuilder.AddColumn<string>(
                name: "AssignerId",
                table: "Leads",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalesPersonId",
                table: "Leads",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Leads_AssignerId",
                table: "Leads",
                column: "AssignerId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_SalesPersonId",
                table: "Leads",
                column: "SalesPersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_Users_AssignerId",
                table: "Leads",
                column: "AssignerId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_Users_SalesPersonId",
                table: "Leads",
                column: "SalesPersonId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leads_Users_AssignerId",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_Users_SalesPersonId",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_AssignerId",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_SalesPersonId",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "AssignerId",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "SalesPersonId",
                table: "Leads");

            migrationBuilder.AddColumn<string>(
                name: "Assigner",
                table: "Leads",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SellsPerson",
                table: "Leads",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
