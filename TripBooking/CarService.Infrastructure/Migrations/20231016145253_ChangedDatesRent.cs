using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarService.Infrastructure.Migrations
{
    public partial class ChangedDatesRent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DropOffTime",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "PickUpTime",
                table: "Rents");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeparturePickUp",
                table: "Rents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnPickUp",
                table: "Rents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeparturePickUp",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "ReturnPickUp",
                table: "Rents");

            migrationBuilder.AddColumn<string>(
                name: "DropOffTime",
                table: "Rents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PickUpTime",
                table: "Rents",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
