using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterportCargoWPF.Migrations
{
    public partial class AddAdditionalRequirementsToQuotation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalRequirements",
                table: "Quotations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalRequirements",
                table: "Quotations");
        }
    }
}
