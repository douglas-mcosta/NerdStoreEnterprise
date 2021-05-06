using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services;
using System;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Controllers
{
    public class CarrinhoController : MainController
    {
        private readonly ICatalogoService _catalogoService;
        private readonly ICarrinhoService _carrinhoService;

        public CarrinhoController(ICatalogoService catalogoService, ICarrinhoService carrinhoService)
        {
            _catalogoService = catalogoService;
            _carrinhoService = carrinhoService;
        }

        [Route("carrinho")]
        public async Task<IActionResult> Index()
        {
            return View(await _carrinhoService.ObterCarrinho());
        }

        [HttpPost]
        [Route("carrinho/adicionar-item")]
        public async Task<IActionResult> AdicionarItemCarrinho(ItemProdutoViewModel itemProduto)
        {
            var produto = await _catalogoService.ObterPorId(itemProduto.ProdutoId);
            var carrinho = await _carrinhoService.ObterCarrinho();

            ValidarItemCarrinho(produto, itemProduto.Quantidade);
            if (!OperecaoValida()) return View("Index", carrinho);

            itemProduto.Nome = produto.Nome;
            itemProduto.Imagem = produto.Imagem;
            itemProduto.Valor = produto.Valor;

            var response = await _carrinhoService.AdicionarItemCarrinho(itemProduto);

            if (ResponsePossuiErros(response))
            {                
                return View("Index", carrinho);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("carrinho/atualizar-item")]
        public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, int quantidade)
        {
            var produto = await _catalogoService.ObterPorId(produtoId);
            var carrinho = await _carrinhoService.ObterCarrinho();

            ValidarItemCarrinho(produto, quantidade);
            if (!OperecaoValida()) return View("Index", carrinho);

            var itemProduto = new ItemProdutoViewModel { ProdutoId = produto.Id, Quantidade = quantidade };

            var response = await _carrinhoService.AtualizarItemCarrinho(produtoId,itemProduto);

            if (ResponsePossuiErros(response))
            {
                return View("Index", carrinho);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("carrinho/remover-item")]
        public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
        {
            var produto = await _catalogoService.ObterPorId(produtoId);
            var carrinho = await _carrinhoService.ObterCarrinho();

            if (produto is null)
            {
                AdicionaErroValidacao("Produto inexistente.");
                return View("Index", carrinho);
            }

            var response = await _carrinhoService.RemoverItemCarrinho(produtoId);

            if (ResponsePossuiErros(response)) return View("Index", carrinho);

            return RedirectToAction("Index");
        }

        private void ValidarItemCarrinho(ProdutoViewModel produto, int quantidade)
        {
            if (produto is null) AdicionaErroValidacao("Produto inexistente.");
            if (quantidade < 1) AdicionaErroValidacao($"Unidade para o produto {produto.Nome} está abaixo do permitido.");
            if (quantidade > produto.QuantidadeEstoque) AdicionaErroValidacao($"Possuimos apenas {produto.QuantidadeEstoque} {produto.Nome} em estoque, você selecionou {quantidade}!");

        }
    }
}