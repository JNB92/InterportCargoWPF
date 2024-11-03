using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterportCargoWPF.Migrations
{
    public partial class AddDiscountColumnToQuotation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "Quotations",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FinalAmount",
                table: "Quotations",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "Quotations",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "RateSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ContainerType = table.Column<string>(type: "TEXT", nullable: false),
                    WharfBookingFee = table.Column<decimal>(type: "TEXT", nullable: false),
                    LiftOnLiftOffFee = table.Column<decimal>(type: "TEXT", nullable: false),
                    FumigationFee = table.Column<decimal>(type: "TEXT", nullable: false),
                    LclDeliveryDepot = table.Column<decimal>(type: "TEXT", nullable: false),
                    TailgateInspectionFee = table.Column<decimal>(type: "TEXT", nullable: false),
                    StorageFee = table.Column<decimal>(type: "TEXT", nullable: false),
                    FacilityFee = table.Column<decimal>(type: "TEXT", nullable: false),
                    WharfInspectionFee = table.Column<decimal>(type: "TEXT", nullable: false),
                    GstRate = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RateSchedules", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RateSchedules");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Quotations");

            migrationBuilder.DropColumn(
                name: "FinalAmount",
                table: "Quotations");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Quotations");
        }
    }
}
