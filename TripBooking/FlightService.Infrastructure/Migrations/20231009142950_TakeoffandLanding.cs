using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightService.Infrastructure.Migrations
{
    public partial class TakeoffandLanding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LengthInMinutes",
                table: "Flights");

            migrationBuilder.RenameColumn(
                name: "ReturnTime",
                table: "Flights",
                newName: "ReturnTakeoffTime");

            migrationBuilder.RenameColumn(
                name: "DepartureTime",
                table: "Flights",
                newName: "ReturnLandingTime");

            migrationBuilder.AddColumn<string>(
                name: "DepartureLandingTime",
                table: "Flights",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartureTakeoffTime",
                table: "Flights",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartureLandingTime",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "DepartureTakeoffTime",
                table: "Flights");

            migrationBuilder.RenameColumn(
                name: "ReturnTakeoffTime",
                table: "Flights",
                newName: "ReturnTime");

            migrationBuilder.RenameColumn(
                name: "ReturnLandingTime",
                table: "Flights",
                newName: "DepartureTime");

            migrationBuilder.AddColumn<double>(
                name: "LengthInMinutes",
                table: "Flights",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
