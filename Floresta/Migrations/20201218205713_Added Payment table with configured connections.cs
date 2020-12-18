using Microsoft.EntityFrameworkCore.Migrations;

namespace Floresta.Migrations
{
    public partial class AddedPaymenttablewithconfiguredconnections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlantCount",
                table: "Markers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isPlantingFinished",
                table: "Markers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MarkerId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    isPaymentSucceded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payment_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payment_Markers_MarkerId",
                        column: x => x.MarkerId,
                        principalTable: "Markers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                        name: "FK_PaymentSeedling_Payment_PaymentsId",
                        column: x => x.PaymentsId,
                        principalTable: "Payment",
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
                        name: "FK_SeedlingAmount_Payment_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payment_MarkerId",
                table: "Payment",
                column: "MarkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_UserId",
                table: "Payment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentSeedling_SeedlingsId",
                table: "PaymentSeedling",
                column: "SeedlingsId");

            migrationBuilder.CreateIndex(
                name: "IX_SeedlingAmount_PaymentId",
                table: "SeedlingAmount",
                column: "PaymentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentSeedling");

            migrationBuilder.DropTable(
                name: "SeedlingAmount");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropColumn(
                name: "PlantCount",
                table: "Markers");

            migrationBuilder.DropColumn(
                name: "isPlantingFinished",
                table: "Markers");
        }
    }
}
