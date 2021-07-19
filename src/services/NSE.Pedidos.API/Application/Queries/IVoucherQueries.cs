using System.Threading.Tasks;
using NSE.Pedidos.API.Application.DTO;

namespace NSE.Pedidos.API.Application.Queries
{
    public interface IVoucherQueries
    {
        Task<VoucherDTO> ObterVoucherPorCodigo(string codigo);
    }
}