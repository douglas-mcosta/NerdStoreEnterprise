using Microsoft.AspNetCore.Mvc;
using NSE.Bff.Compras.Models;
using NSE.Bff.Compras.Services;
using NSE.WebApi.Core.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Bff.Compras.Controllers
{
    public class CarrinhoController : MainController
    {
        private readonly ICarrinhoService _carrinhoService;
        private readonly ICatalogoService _catalogoService;
        private readonly IPedidoService _pedidoService;

        public CarrinhoController(ICarrinhoService carrinhoService, ICatalogoService catalogoService, IPedidoService pedidoService)
        {
            _carrinhoService = carrinhoService;
            _catalogoService = catalogoService;
            _pedidoService = pedidoService;
        }

        [HttpPost("compras/carrinho/items")]
        public async Task<IActionResult> AdicionarItemCarrinho(ItemCarrinhoDto itemProdutoCarrinho)
        {
            var produto = await _catalogoService.ObterPorId(itemProdutoCarrinho.ProdutoId);
            await ValidarItemCarrinho(produto, itemProdutoCarrinho.Quantidade);

            if (!OperacaoValida()) return CustomResponse();

            itemProdutoCarrinho.Nome = produto.Nome;
            itemProdutoCarrinho.Imagem = produto.Imagem;
            itemProdutoCarrinho.Valor = produto.Valor;

            var response = await _carrinhoService.AdicionarItemCarrinho(itemProdutoCarrinho);

            return CustomResponse(response);
        }

        [HttpPut("compras/carrinho/items/{produtoId}")]
        public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoDto itemProdutoCarrinho)
        {
            var produto = await _catalogoService.ObterPorId(produtoId);
            await ValidarItemCarrinho(produto, itemProdutoCarrinho.Quantidade);

            if (!OperacaoValida()) return CustomResponse();

            var response = await _carrinhoService.AtualizarItemCarrinho(produtoId, itemProdutoCarrinho);

            return CustomResponse(response);
        }

        [HttpGet("compras/carrinho")]
        public async Task<IActionResult> Index()
        {
            var carrinho = await _carrinhoService.ObterCarrinho();
            return CustomResponse(carrinho);
        }
        [HttpGet]
        [Route("compras/carrinho-quantidade")]
        public async Task<int> ObterQuantidadeCarrinho()
        {
            var carrinho = await _carrinhoService.ObterCarrinho();
            var quantidade = carrinho?.Itens.Sum(i => i.Quantidade) ?? 0;
            return quantidade;
        }
        [HttpDelete("compras/carrinho/items/{produtoId}")]
        public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
        {
            var produto = await _catalogoService.ObterPorId(produtoId);

            if(produto is null)
            {
                AdicionarErroProcessamento("Produto inexistente.");
                return CustomResponse();
            }

            var response = await _carrinhoService.RemoverItemCarrinho(produtoId);
            return CustomResponse(response);
        }

        [HttpPost("compras/carrinho/aplicar-voucher")]
        public async Task<IActionResult> AplicarVoucher([FromBody] string voucherCodigo)
        {
            var voucher = await _pedidoService.ObterVoucherPorCodigo(voucherCodigo);
            if (voucher is null)
            {
                AdicionarErroProcessamento("Voucher inválido ou não encontrado");
                return CustomResponse();
            }

            var response = await _carrinhoService.AplicarVoucherCarrinho(voucher);
            return CustomResponse(response);
        }

        private async Task ValidarItemCarrinho(ItemProdutoDto produto, int quantidade)
        {
            if (produto is null) AdicionarErroProcessamento("Produto inexistente.");
            if (quantidade < 1) AdicionarErroProcessamento($"Escolha ao menos uma unidade do produto {produto.Nome}.");

            var carrinho = await _carrinhoService.ObterCarrinho();
            var itemCarrinho = carrinho.Itens.FirstOrDefault(p => p.ProdutoId == produto.Id);

            var quantidadeTotalDeItens = itemCarrinho?.Quantidade + quantidade;

            if (quantidadeTotalDeItens > produto.QuantidadeEstoque)
            {
                AdicionarErroProcessamento($"O produto {produto.Nome} possui {produto.QuantidadeEstoque} unidades em estoque, você selecionou {quantidade}.");
                return;
            }

            if (quantidade > produto.QuantidadeEstoque) AdicionarErroProcessamento($"Possuímos apenas {produto.QuantidadeEstoque} {produto.Nome} em estoque, você selecionou {quantidade}!");

        }
    }
}