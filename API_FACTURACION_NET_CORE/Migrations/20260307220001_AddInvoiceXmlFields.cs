using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_FACTURACION_NET_CORE.Migrations
{
    /// <inheritdoc />
    public partial class AddInvoiceXmlFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SriAuthorizationDate",
                table: "Invoices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SriAuthorizationNumber",
                table: "Invoices",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "XmlContent",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "XmlPath",
                table: "Invoices",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SriAuthorizationDate",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "SriAuthorizationNumber",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "XmlContent",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "XmlPath",
                table: "Invoices");
        }
    }
}
