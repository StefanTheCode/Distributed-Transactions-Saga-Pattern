using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarService.Infrastructure.Migrations
{
    public partial class ChangedRent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DropOffDate",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "PickUpDate",
                table: "Rents");

            migrationBuilder.RenameColumn(
                name: "DriverAge",
                table: "Rents",
                newName: "MinimumDriverAge");

            migrationBuilder.AddColumn<string>(
                name: "CarDescription",
                table: "Rents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CarName",
                table: "Rents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CarPhoto",
                table: "Rents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DropOffTime",
                table: "Rents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaximumNumOfPassengers",
                table: "Rents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PickUpTime",
                table: "Rents",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarDescription",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "CarName",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "CarPhoto",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "DropOffTime",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "MaximumNumOfPassengers",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "PickUpTime",
                table: "Rents");

            migrationBuilder.RenameColumn(
                name: "MinimumDriverAge",
                table: "Rents",
                newName: "DriverAge");

            migrationBuilder.AddColumn<DateTime>(
                name: "DropOffDate",
                table: "Rents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "PickUpDate",
                table: "Rents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
