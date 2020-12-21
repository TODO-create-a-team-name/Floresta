using Microsoft.EntityFrameworkCore.Migrations;

namespace Floresta.Migrations
{
    public partial class Changedpaymenttoseedlingrelationshipfrommanytomanytoonetomany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentSeedling");

            migrationBuilder.DropTable(
                name: "SeedlingAmount");

            migrationBuilder.RenameColumn(
                name: "isPaymentSucceded",
                table: "Payments",
                newName: "IsPaymentSucceded");

            migrationBuilder.AddColumn<int>(
                name: "PurchasedAmount",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SeedlingId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_SeedlingId",
                table: "Payments",
                column: "SeedlingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Seedlings_SeedlingId",
                table: "Payments",
                column: "SeedlingId",
                principalTable: "Seedlings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Seedlings_SeedlingId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_SeedlingId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PurchasedAmount",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "SeedlingId",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "IsPaymentSucceded",
                table: "Payments",
                newName: "isPaymentSucceded");

            migrationBuilder.CreateTable(
                name: "PaymentSeedling",
                columns: table => new
                {
                    PaymentsId = table.Column<int>(type: "int", nullable: false),
                    SeedlingsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentSeedling", x => new { x.PaymentsId, x.SeedlingsId });
                    table.ForeignKey(
                        name: "FK_PaymentSeedling_Payments_PaymentsId",
                        column: x => x.PaymentsId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentSeedling_Seedlings_SeedlingsId",
                        column: x => x.SeedlingsId,
                        principalTable: "Seedlings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeedlingAmount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    PaymentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeedlingAmount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeedlingAmount_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentSeedling_SeedlingsId",
                table: "PaymentSeedling",
                column: "SeedlingsId");

            migrationBuilder.CreateIndex(
                name: "IX_SeedlingAmount_PaymentId",
                table: "SeedlingAmount",
                column: "PaymentId");
        }
    }
}
