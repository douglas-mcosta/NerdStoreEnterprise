using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services;
using System;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Controllers
{
    [Authorize]
    public class CarrinhoController : MainController
    {
        private readonly IComprasBffService _comprasBffService;

        public CarrinhoController(IComprasBffService carrinhoService)
        {
            _comprasBffService = carrinhoService;
        }

        [Route("carrinho")]
        public async Task<IActionResult> Index()
        {
            return View(await _comprasBffService.ObterCarrinho());
        }

        [HttpPost]
        [Route("carrinho/adicionar-item")]
        public async Task<IActionResult> AdicionarItemCarrinho(ItemCarrinhoViewModel itemCarrinho)
        {
            var response = await _comprasBffService.AdicionarItemCarrinho(itemCarrinho);

            if (ResponsePossuiErros(response))
            {
                var carrinho = await _comprasBffService.ObterCarrinho();
                return View("Index", carrinho);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("carrinho/atualizar-item")]
        public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, int quantidade)
        {
            var itemCarrinho = new ItemCarrinhoViewModel { ProdutoId = produtoId, Quantidade = quantidade };
            var response = await _comprasBffService.AtualizarItemCarrinho(produtoId, itemCarrinho);

            if (ResponsePossuiErros(response))
            {
                var carrinho = await _comprasBffService.ObterCarrinho();
                return View("Index", carrinho);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("carrinho/remover-item")]
        public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
        {
            var response = await _comprasBffService.RemoverItemCarrinho(produtoId);

            if (ResponsePossuiErros(response))
            {
                var carrinho = await _comprasBffService.ObterCarrinho();
                return View("Index", carrinho);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("carrinho/aplicar-voucher")]
        public async Task<IActionResult> AplicarVoucher(string voucherCodigo)
        {
            var response = await _comprasBffService.AplicarVoucher(voucherCodigo);
            var carrinho = await _comprasBffService.ObterCarrinho();

            if (ResponsePossuiErros(response)) return View("Index", carrinho);

            return RedirectToAction("Index");
        }
    }
}