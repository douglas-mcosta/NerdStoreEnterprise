using System;
using NSE.Core.Messages;

namespace NSE.Pedidos.API.Application.Events
{
    public class PedidoRealizadoEvent : Event
    {
        public Guid PedidoId { get; private set; }
        public Guid ClientId { get; private set; }

        public PedidoRealizadoEvent(Guid pedidoId, Guid clientId)
        {
            PedidoId = pedidoId;
            ClientId = clientId;
        }
    }
}