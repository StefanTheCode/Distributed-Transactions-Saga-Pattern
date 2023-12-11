using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Saga.Consumer.Migrations
{
    public partial class AddedKeyId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingStates",
                table: "BookingStates");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "BookingStates",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingStates",
                table: "BookingStates",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingStates",
                table: "BookingStates");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BookingStates");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingStates",
                table: "BookingStates",
                column: "CorrelationId");
        }
    }
}
