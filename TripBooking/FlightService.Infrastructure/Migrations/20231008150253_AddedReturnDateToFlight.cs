using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightService.Infrastructure.Migrations
{
    public partial class AddedReturnDateToFlight : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ArrivalTime",
                table: "Flights",
                newName: "ReturnTime");

            migrationBuilder.RenameColumn(
                name: "Arrival",
                table: "Flights",
                newName: "Return");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReturnTime",
                table: "Flights",
                newName: "ArrivalTime");

            migrationBuilder.RenameColumn(
                name: "Return",
                table: "Flights",
                newName: "Arrival");
        }
    }
}
