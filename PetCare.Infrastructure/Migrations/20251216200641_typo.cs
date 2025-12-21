using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class typo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Procedures_VetSpecializations_VetSpezializationId",
                table: "Procedures");

            migrationBuilder.RenameColumn(
                name: "VetSpezializationId",
                table: "Procedures",
                newName: "VetSpecializationId");

            migrationBuilder.RenameIndex(
                name: "IX_Procedures_VetSpezializationId",
                table: "Procedures",
                newName: "IX_Procedures_VetSpecializationId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AddForeignKey(
                name: "FK_Procedures_VetSpecializations_VetSpecializationId",
                table: "Procedures",
                column: "VetSpecializationId",
                principalTable: "VetSpecializations",
                principalColumn: "VetSpecializationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Procedures_VetSpecializations_VetSpecializationId",
                table: "Procedures");

            migrationBuilder.RenameColumn(
                name: "VetSpecializationId",
                table: "Procedures",
                newName: "VetSpezializationId");

            migrationBuilder.RenameIndex(
                name: "IX_Procedures_VetSpecializationId",
                table: "Procedures",
                newName: "IX_Procedures_VetSpezializationId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Procedures_VetSpecializations_VetSpezializationId",
                table: "Procedures",
                column: "VetSpezializationId",
                principalTable: "VetSpecializations",
                principalColumn: "VetSpecializationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
