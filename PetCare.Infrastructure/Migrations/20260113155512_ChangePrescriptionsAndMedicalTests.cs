using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangePrescriptionsAndMedicalTests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Procedures_VetSpecializations_VetSpecializationId",
                table: "Procedures");

            migrationBuilder.AddColumn<DateOnly>(
                name: "CreatedDate",
                table: "Prescriptions",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AlterColumn<string>(
                name: "Result",
                table: "MedicalTests",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "AttachmentUrl",
                table: "MedicalTests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Procedures_VetSpecializations_VetSpecializationId",
                table: "Procedures",
                column: "VetSpecializationId",
                principalTable: "VetSpecializations",
                principalColumn: "VetSpecializationId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Procedures_VetSpecializations_VetSpecializationId",
                table: "Procedures");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Prescriptions");

            migrationBuilder.AlterColumn<string>(
                name: "Result",
                table: "MedicalTests",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AttachmentUrl",
                table: "MedicalTests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Procedures_VetSpecializations_VetSpecializationId",
                table: "Procedures",
                column: "VetSpecializationId",
                principalTable: "VetSpecializations",
                principalColumn: "VetSpecializationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
