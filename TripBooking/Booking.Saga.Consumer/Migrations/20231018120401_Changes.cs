using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Saga.Consumer.Migrations
{
    public partial class Changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSuccessful",
                table: "BookingStates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "BookingStates",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSuccessful",
                table: "BookingStates");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "BookingStates");
        }
    }
}
