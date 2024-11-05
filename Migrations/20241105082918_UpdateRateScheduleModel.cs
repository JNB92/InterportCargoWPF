using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterportCargoWPF.Migrations
{
    public partial class UpdateRateScheduleModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContainerType",
                table: "RateSchedules");

            migrationBuilder.DropColumn(
                name: "FacilityFee",
                table: "RateSchedules");

            migrationBuilder.DropColumn(
                name: "FumigationFee",
                table: "RateSchedules");

            migrationBuilder.DropColumn(
                name: "GstRate",
                table: "RateSchedules");

            migrationBuilder.DropColumn(
                name: "LclDeliveryDepot",
                table: "RateSchedules");

            migrationBuilder.DropColumn(
                name: "LiftOnLiftOffFee",
                table: "RateSchedules");

            migrationBuilder.DropColumn(
                name: "StorageFee",
                table: "RateSchedules");

            migrationBuilder.RenameColumn(
                name: "WharfInspectionFee",
                table: "RateSchedules",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "WharfBookingFee",
                table: "RateSchedules",
                newName: "TwentyFeetContainer");

            migrationBuilder.RenameColumn(
                name: "TailgateInspectionFee",
                table: "RateSchedules",
                newName: "FortyFeetContainer");

            migrationBuilder.InsertData(
                table: "RateSchedules",
                columns: new[] { "Id", "FortyFeetContainer", "TwentyFeetContainer", "Type" },
                values: new object[] { 1, 70m, 60m, "Wharf Booking Fee" });

            migrationBuilder.InsertData(
                table: "RateSchedules",
                columns: new[] { "Id", "FortyFeetContainer", "TwentyFeetContainer", "Type" },
                values: new object[] { 2, 120m, 80m, "Lift On/Lift Off" });

            migrationBuilder.InsertData(
                table: "RateSchedules",
                columns: new[] { "Id", "FortyFeetContainer", "TwentyFeetContainer", "Type" },
                values: new object[] { 3, 280m, 220m, "Fumigation" });

            migrationBuilder.InsertData(
                table: "RateSchedules",
                columns: new[] { "Id", "FortyFeetContainer", "TwentyFeetContainer", "Type" },
                values: new object[] { 4, 500m, 400m, "LCL Delivery Depot" });

            migrationBuilder.InsertData(
                table: "RateSchedules",
                columns: new[] { "Id", "FortyFeetContainer", "TwentyFeetContainer", "Type" },
                values: new object[] { 5, 160m, 120m, "Tailgate Inspection" });

            migrationBuilder.InsertData(
                table: "RateSchedules",
                columns: new[] { "Id", "FortyFeetContainer", "TwentyFeetContainer", "Type" },
                values: new object[] { 6, 300m, 240m, "Storage Fee" });

            migrationBuilder.InsertData(
                table: "RateSchedules",
                columns: new[] { "Id", "FortyFeetContainer", "TwentyFeetContainer", "Type" },
                values: new object[] { 7, 100m, 70m, "Facility Fee" });

            migrationBuilder.InsertData(
                table: "RateSchedules",
                columns: new[] { "Id", "FortyFeetContainer", "TwentyFeetContainer", "Type" },
                values: new object[] { 8, 90m, 60m, "Wharf Inspection" });

            migrationBuilder.InsertData(
                table: "RateSchedules",
                columns: new[] { "Id", "FortyFeetContainer", "TwentyFeetContainer", "Type" },
                values: new object[] { 9, 10m, 10m, "GST" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RateSchedules",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RateSchedules",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RateSchedules",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RateSchedules",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "RateSchedules",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "RateSchedules",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "RateSchedules",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "RateSchedules",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "RateSchedules",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "RateSchedules",
                newName: "WharfInspectionFee");

            migrationBuilder.RenameColumn(
                name: "TwentyFeetContainer",
                table: "RateSchedules",
                newName: "WharfBookingFee");

            migrationBuilder.RenameColumn(
                name: "FortyFeetContainer",
                table: "RateSchedules",
                newName: "TailgateInspectionFee");

            migrationBuilder.AddColumn<string>(
                name: "ContainerType",
                table: "RateSchedules",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "FacilityFee",
                table: "RateSchedules",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FumigationFee",
                table: "RateSchedules",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "GstRate",
                table: "RateSchedules",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LclDeliveryDepot",
                table: "RateSchedules",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LiftOnLiftOffFee",
                table: "RateSchedules",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "StorageFee",
                table: "RateSchedules",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
