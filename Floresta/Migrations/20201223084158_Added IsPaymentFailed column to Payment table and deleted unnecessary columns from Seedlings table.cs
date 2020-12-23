using Microsoft.EntityFrameworkCore.Migrations;

namespace Floresta.Migrations
{
    public partial class AddedIsPaymentFailedcolumntoPaymenttableanddeletedunnecessarycolumnsfromSeedlingstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Seedlings");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Seedlings");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaymentFailed",
                table: "Payments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaymentFailed",
                table: "Payments");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Seedlings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Height",
                table: "Seedlings",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
