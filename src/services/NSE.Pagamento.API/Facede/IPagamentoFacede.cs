using NSE.Pagamentos.API.Models;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace NSE.Pagamentos.API.Facede
{
    public interface IPagamentoFacede
    {
        Task<Transacao> AutorizarPagamento(Pagamento pagamento);
         Task<Transacao> CapturarPagamento(Transacao transacao);
        Task<Transacao> CancelarAutorizacao(Transacao transacao);
    }
}
