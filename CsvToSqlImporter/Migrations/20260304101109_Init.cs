using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CsvToSqlImporter.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppTrips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TpepPickupDatetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TpepDropoffDatetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PassengerCount = table.Column<int>(type: "int", nullable: true),
                    TripDistance = table.Column<double>(type: "float", nullable: false),
                    StoreAndFwdFlag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PULocationID = table.Column<int>(type: "int", nullable: false),
                    DOLocationID = table.Column<int>(type: "int", nullable: false),
                    FareAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TipAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTrips", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppTrips_PULocationID",
                table: "AppTrips",
                column: "PULocationID");

            migrationBuilder.CreateIndex(
                name: "IX_AppTrips_TripDistance",
                table: "AppTrips",
                column: "TripDistance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppTrips");
        }
    }
}
