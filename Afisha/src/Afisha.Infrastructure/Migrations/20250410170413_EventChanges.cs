using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Afisha.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EventChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "EventUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOpenToRegister",
                table: "Events",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "EventUsers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "IsApproved",
                value: false);

            migrationBuilder.UpdateData(
                table: "EventUsers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "IsApproved",
                value: false);

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1L,
                column: "IsOpenToRegister",
                value: false);

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2L,
                column: "IsOpenToRegister",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "EventUsers");

            migrationBuilder.DropColumn(
                name: "IsOpenToRegister",
                table: "Events");
        }
    }
}
