using Microsoft.EntityFrameworkCore;
using NSE.Core.DomainObjects.Data;
using NSE.Pedidos.Domain.Vouchers;
using System.Threading.Tasks;

namespace NSE.Pedidos.Infra.Data.Repository
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly PedidoContext _context;

        public VoucherRepository(PedidoContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<Voucher> ObterVoucherPorCodigo(string codigo)
        {
            return await _context.Voucher.FirstOrDefaultAsync(v => v.Codigo == codigo);
        }

        public void Atualizar(Voucher voucher)
        {
            _context.Voucher.Update(voucher);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}