using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Migrations
{
    /// <inheritdoc />
    public partial class OrganixationIDFortigenKeyAppliedInLeadsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrganizationId",
                table: "Leads",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_OrganizationId",
                table: "Leads",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_Users_OrganizationId",
                table: "Leads",
                column: "OrganizationId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leads_Users_OrganizationId",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_OrganizationId",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Leads");
        }
    }
}
