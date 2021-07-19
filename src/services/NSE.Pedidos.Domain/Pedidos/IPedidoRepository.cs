using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using NSE.Core.DomainObjects.Data;

namespace NSE.Pedidos.Domain.Pedidos
{
    public interface IPedidoRepository: IRepository<Pedido>
    {
        #region Pedido

        DbConnection ObterConexao();
        Task<Pedido> ObterPorId(Guid id);
        Task<IEnumerable<Pedido>> ObterListaPorClientId(Guid clienteId);
        void Adicionar(Pedido pedido);
        void Atualizar(Pedido pedido);

        #endregion

        #region PedidoItem

        Task<PedidoItem> ObterItemPorId(Guid id);
        Task<PedidoItem> ObterItemPorPedido(Guid pedidoId, Guid produtoId);

        #endregion
    }
}