using Microsoft.EntityFrameworkCore.Migrations;

namespace NSE.Pedidos.Infra.Migrations
{
    public partial class AjusteNomes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataAtualizacao",
                table: "Vouchers",
                newName: "DataUtilizacao");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Pedidos",
                newName: "ClienteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataUtilizacao",
                table: "Vouchers",
                newName: "DataAtualizacao");

            migrationBuilder.RenameColumn(
                name: "ClienteId",
                table: "Pedidos",
                newName: "ClientId");
        }
    }
}
