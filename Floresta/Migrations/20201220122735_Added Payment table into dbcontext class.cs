using Microsoft.EntityFrameworkCore.Migrations;

namespace Floresta.Migrations
{
    public partial class AddedPaymenttableintodbcontextclass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_AspNetUsers_UserId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Markers_MarkerId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentSeedling_Payment_PaymentsId",
                table: "PaymentSeedling");

            migrationBuilder.DropForeignKey(
                name: "FK_SeedlingAmount_Payment_PaymentId",
                table: "SeedlingAmount");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payment",
                table: "Payment");

            migrationBuilder.RenameTable(
                name: "Payment",
                newName: "Payments");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_UserId",
                table: "Payments",
                newName: "IX_Payments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_MarkerId",
                table: "Payments",
                newName: "IX_Payments_MarkerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payments",
                table: "Payments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AspNetUsers_UserId",
                table: "Payments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Markers_MarkerId",
                table: "Payments",
                column: "MarkerId",
                principalTable: "Markers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentSeedling_Payments_PaymentsId",
                table: "PaymentSeedling",
                column: "PaymentsId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeedlingAmount_Payments_PaymentId",
                table: "SeedlingAmount",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_UserId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Markers_MarkerId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentSeedling_Payments_PaymentsId",
                table: "PaymentSeedling");

            migrationBuilder.DropForeignKey(
                name: "FK_SeedlingAmount_Payments_PaymentId",
                table: "SeedlingAmount");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payments",
                table: "Payments");

            migrationBuilder.RenameTable(
                name: "Payments",
                newName: "Payment");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_UserId",
                table: "Payment",
                newName: "IX_Payment_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_MarkerId",
                table: "Payment",
                newName: "IX_Payment_MarkerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payment",
                table: "Payment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_AspNetUsers_UserId",
                table: "Payment",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Markers_MarkerId",
                table: "Payment",
                column: "MarkerId",
                principalTable: "Markers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentSeedling_Payment_PaymentsId",
                table: "PaymentSeedling",
                column: "PaymentsId",
                principalTable: "Payment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeedlingAmount_Payment_PaymentId",
                table: "SeedlingAmount",
                column: "PaymentId",
                principalTable: "Payment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
