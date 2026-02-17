using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDatabaseObjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE FUNCTION dbo.fn_CalculatePetAge (@BirthDate DATE)
                RETURNS NVARCHAR(50)
                AS
                BEGIN
                    DECLARE @Age NVARCHAR(50)
                    DECLARE @Years INT = DATEDIFF(YEAR, @BirthDate, GETDATE())
    
                    IF (DATEADD(YEAR, @Years, @BirthDate) > GETDATE())
                        SET @Years = @Years - 1

                    IF @Years > 0
                        SET @Age = CAST(@Years AS NVARCHAR(10)) + ' years'
                    ELSE
                        SET @Age = CAST(DATEDIFF(MONTH, @BirthDate, GETDATE()) AS NVARCHAR(10)) + ' months'

                    RETURN @Age
                END
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_GetVetStatistics
                    @VetId INT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DECLARE @StartOfMonth DATE = DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1);

                    SELECT 
                        (SELECT COUNT(*) FROM Appointments WHERE VetId = @VetId) AS TotalAppointments,
                        (SELECT COUNT(*) FROM Appointments WHERE VetId = @VetId AND AppointmentDateTime >= @StartOfMonth) AS AppointmentsThisMonth,
                        (SELECT COUNT(DISTINCT PetId) FROM Appointments WHERE VetId = @VetId) AS UniquePatients,
                        (SELECT ISNULL(SUM(TotalAmount), 0) FROM Invoices i JOIN Appointments a ON i.AppointmentId = a.AppointmentId WHERE a.VetId = @VetId) AS TotalRevenue
                END
            ");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AppointmentDateTime",
                table: "Appointments",
                column: "AppointmentDateTime");

            migrationBuilder.Sql(@"
                ALTER TABLE Pets
                ADD AgeDescription AS dbo.fn_CalculatePetAge(DateOfBirth);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE Pets DROP COLUMN AgeDescription");
            migrationBuilder.Sql("DROP PROCEDURE sp_GetVetStatistics");
            migrationBuilder.Sql("DROP FUNCTION dbo.fn_CalculatePetAge");
            migrationBuilder.DropIndex(name: "IX_Appointments_AppointmentDateTime", table: "Appointments");
        }
    }
}
