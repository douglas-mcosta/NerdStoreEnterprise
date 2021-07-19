using Microsoft.EntityFrameworkCore.Migrations;

namespace NSE.Catalogo.API.Migrations
{
    public partial class TesteDeAlteracao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "produtos",
                type: "varchar(300)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(250)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "produtos",
                type: "varchar(250)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(300)");
        }
    }
}
