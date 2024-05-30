using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Migrations
{
    /// <inheritdoc />
    public partial class DataEntryOpratorForigenKeyAppliedInLeadsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataEnteryOprator",
                table: "Leads");

            migrationBuilder.AddColumn<string>(
                name: "DataEnteryOpratorId",
                table: "Leads",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_DataEnteryOpratorId",
                table: "Leads",
                column: "DataEnteryOpratorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_Users_DataEnteryOpratorId",
                table: "Leads",
                column: "DataEnteryOpratorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leads_Users_DataEnteryOpratorId",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_DataEnteryOpratorId",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "DataEnteryOpratorId",
                table: "Leads");

            migrationBuilder.AddColumn<string>(
                name: "DataEnteryOprator",
                table: "Leads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
