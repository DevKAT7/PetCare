using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class VaccinationsDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vaccination_Appointments_AppointmentId",
                table: "Vaccination");

            migrationBuilder.DropForeignKey(
                name: "FK_Vaccination_Pets_PetId",
                table: "Vaccination");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vaccination",
                table: "Vaccination");

            migrationBuilder.RenameTable(
                name: "Vaccination",
                newName: "Vaccinations");

            migrationBuilder.RenameIndex(
                name: "IX_Vaccination_PetId",
                table: "Vaccinations",
                newName: "IX_Vaccinations_PetId");

            migrationBuilder.RenameIndex(
                name: "IX_Vaccination_AppointmentId",
                table: "Vaccinations",
                newName: "IX_Vaccinations_AppointmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vaccinations",
                table: "Vaccinations",
                column: "VaccinationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vaccinations_Appointments_AppointmentId",
                table: "Vaccinations",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "AppointmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vaccinations_Pets_PetId",
                table: "Vaccinations",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "PetId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vaccinations_Appointments_AppointmentId",
                table: "Vaccinations");

            migrationBuilder.DropForeignKey(
                name: "FK_Vaccinations_Pets_PetId",
                table: "Vaccinations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vaccinations",
                table: "Vaccinations");

            migrationBuilder.RenameTable(
                name: "Vaccinations",
                newName: "Vaccination");

            migrationBuilder.RenameIndex(
                name: "IX_Vaccinations_PetId",
                table: "Vaccination",
                newName: "IX_Vaccination_PetId");

            migrationBuilder.RenameIndex(
                name: "IX_Vaccinations_AppointmentId",
                table: "Vaccination",
                newName: "IX_Vaccination_AppointmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vaccination",
                table: "Vaccination",
                column: "VaccinationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vaccination_Appointments_AppointmentId",
                table: "Vaccination",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "AppointmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vaccination_Pets_PetId",
                table: "Vaccination",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "PetId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
