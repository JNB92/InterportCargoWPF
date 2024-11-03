using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterportCargoWPF.Migrations
{
    public partial class AddTransportationDateToQuotation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TransportationDate",
                table: "Quotations",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransportationDate",
                table: "Quotations");
        }
    }
}
