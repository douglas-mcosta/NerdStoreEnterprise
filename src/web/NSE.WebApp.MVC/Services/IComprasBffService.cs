using NSE.Core.Communication;
using NSE.WebApp.MVC.Models;
using System;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    public interface IComprasBffService
    {
        Task<CarrinhoViewModel> ObterCarrinho();

        Task<int> ObterQuantidadeItensCarrinho();
        Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoViewModel itemCarrinho);

        Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoViewModel itemCarrinho);

        Task<ResponseResult> RemoverItemCarrinho(Guid produtoId);
        Task<ResponseResult> AplicarVoucher(string voucher);
    }
}