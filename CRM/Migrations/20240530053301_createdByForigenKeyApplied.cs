using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Migrations
{
    /// <inheritdoc />
    public partial class createdByForigenKeyApplied : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "LeadNotes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "LeadNotes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeadNotes_CreatedById",
                table: "LeadNotes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LeadNotes_UpdatedById",
                table: "LeadNotes",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_LeadNotes_Users_CreatedById",
                table: "LeadNotes",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeadNotes_Users_UpdatedById",
                table: "LeadNotes",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeadNotes_Users_CreatedById",
                table: "LeadNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_LeadNotes_Users_UpdatedById",
                table: "LeadNotes");

            migrationBuilder.DropIndex(
                name: "IX_LeadNotes_CreatedById",
                table: "LeadNotes");

            migrationBuilder.DropIndex(
                name: "IX_LeadNotes_UpdatedById",
                table: "LeadNotes");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "LeadNotes");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "LeadNotes");
        }
    }
}
