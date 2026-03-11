using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_FACTURACION_NET_CORE.Migrations
{
    /// <inheritdoc />
    public partial class AddClientDataToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Identificacion",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreComercial",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "P12FilePath",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "P12Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RazonSocial",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Identificacion",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NombreComercial",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "P12FilePath",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "P12Password",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RazonSocial",
                table: "Users");
        }
    }
}
