using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightService.Infrastructure.Migrations
{
    public partial class LengthInMinutes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "LengthInMinutes",
                table: "Flights",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LengthInMinutes",
                table: "Flights");
        }
    }
}
