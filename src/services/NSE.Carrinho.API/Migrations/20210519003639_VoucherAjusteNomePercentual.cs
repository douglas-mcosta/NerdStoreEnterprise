using Microsoft.EntityFrameworkCore.Migrations;

namespace NSE.Carrinho.API.Migrations
{
    public partial class VoucherAjusteNomePercentual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Voucher_Percentual",
                table: "CarrinhoCliente",
                newName: "Percentual");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Percentual",
                table: "CarrinhoCliente",
                newName: "Voucher_Percentual");
        }
    }
}
