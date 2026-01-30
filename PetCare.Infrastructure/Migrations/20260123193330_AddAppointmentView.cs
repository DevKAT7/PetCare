using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAppointmentView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW View_AppointmentDetails AS
                SELECT 
                    a.AppointmentId,
                    a.AppointmentDateTime,
                    a.Description,
                    CAST(a.Status AS nvarchar(50)) AS Status,
                    a.ReasonForVisit,
                    a.Diagnosis,
                    a.Notes,
            
                    p.PetId,
                    p.Name AS PetName,
                    p.Species AS PetSpecies,
                    p.ImageUrl AS PetImageUrl,
            
                    po.FirstName + ' ' + po.LastName AS OwnerName,
            
                    v.VetId,
                    v.FirstName + ' ' + v.LastName AS VetName,
            
                    i.InvoiceId
            
                FROM Appointments a
                JOIN Pets p ON a.PetId = p.PetId
                JOIN PetOwners po ON p.PetOwnerId = po.PetOwnerId
                JOIN Vets v ON a.VetId = v.VetId
                LEFT JOIN Invoices i ON a.AppointmentId = i.AppointmentId
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW View_AppointmentDetails");
        }
    }
}
