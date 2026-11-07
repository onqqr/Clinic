using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinic.Services.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueNameIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Services_SpecializationId",
                table: "Services");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_Name",
                table: "Specializations",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_SpecializationId_Name",
                table: "Services",
                columns: new[] { "SpecializationId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Specializations_Name",
                table: "Specializations");

            migrationBuilder.DropIndex(
                name: "IX_Services_SpecializationId_Name",
                table: "Services");

            migrationBuilder.CreateIndex(
                name: "IX_Services_SpecializationId",
                table: "Services",
                column: "SpecializationId");
        }
    }
}
