using NSE.Core.Communication;
using NSE.WebApp.MVC.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    public interface IComprasBffService
    {
        #region Carrinho
        Task<CarrinhoViewModel> ObterCarrinho();

        Task<int> ObterQuantidadeItensCarrinho();
        Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoViewModel itemCarrinho);

        Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoViewModel itemCarrinho);

        Task<ResponseResult> RemoverItemCarrinho(Guid produtoId);
        Task<ResponseResult> AplicarVoucher(string voucher);
        #endregion

        #region Pedido
        Task<ResponseResult> FinalizarPedido(PedidoTransacaoViewModel pedidoTransacao);
        Task<PedidoViewModel> ObterUltimoPedido();
        Task<IEnumerable<PedidoViewModel>> ObterListaPorClienteId();
        PedidoTransacaoViewModel MapearParaPedido(CarrinhoViewModel carrinho, EnderecoViewModel endereco);
        #endregion
    }
}