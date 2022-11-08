using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class somechanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CuentaAhorros_Products_IdProduct",
                table: "CuentaAhorros");

            migrationBuilder.DropForeignKey(
                name: "FK_Prestamos_Products_IdProduct",
                table: "Prestamos");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_IdUser",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_TarjetaCreditos_Products_Idproduct",
                table: "TarjetaCreditos");

            migrationBuilder.AddForeignKey(
                name: "FK_CuentaAhorros_Products_IdProduct",
                table: "CuentaAhorros",
                column: "IdProduct",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prestamos_Products_IdProduct",
                table: "Prestamos",
                column: "IdProduct",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_IdUser",
                table: "Products",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TarjetaCreditos_Products_Idproduct",
                table: "TarjetaCreditos",
                column: "Idproduct",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CuentaAhorros_Products_IdProduct",
                table: "CuentaAhorros");

            migrationBuilder.DropForeignKey(
                name: "FK_Prestamos_Products_IdProduct",
                table: "Prestamos");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_IdUser",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_TarjetaCreditos_Products_Idproduct",
                table: "TarjetaCreditos");

            migrationBuilder.AddForeignKey(
                name: "FK_CuentaAhorros_Products_IdProduct",
                table: "CuentaAhorros",
                column: "IdProduct",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Prestamos_Products_IdProduct",
                table: "Prestamos",
                column: "IdProduct",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_IdUser",
                table: "Products",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TarjetaCreditos_Products_Idproduct",
                table: "TarjetaCreditos",
                column: "Idproduct",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
