using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Saga.Consumer.Migrations
{
    public partial class ChangedBookingState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FlightId",
                table: "BookingStates",
                newName: "FlightDetailsId");

            migrationBuilder.RenameColumn(
                name: "CarId",
                table: "BookingStates",
                newName: "CarDetailsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FlightDetailsId",
                table: "BookingStates",
                newName: "FlightId");

            migrationBuilder.RenameColumn(
                name: "CarDetailsId",
                table: "BookingStates",
                newName: "CarId");
        }
    }
}
