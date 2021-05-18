using NSE.Bff.Compras.Models;
using NSE.Core.Communication;
using System;
using System.Threading.Tasks;

namespace NSE.Bff.Compras.Services
{
    public interface ICarrinhoService
    {
        Task<CarrinhoDto> ObterCarrinho();

        Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoDto produto);

        Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoDto produto);

        Task<ResponseResult> RemoverItemCarrinho(Guid produtoId);
    }
}