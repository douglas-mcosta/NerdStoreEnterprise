using System;
using System.Collections.Generic;

namespace NSE.Carrinho.API.Model
{
    public class CarrinhoCliente
    {
        protected CarrinhoCliente() { }
        public CarrinhoCliente(Guid clienteId)
        {
            Id = Guid.NewGuid();
            ClienteId = clienteId;
            Items = new List<CarrinhoItem>();
        }

        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public decimal ValorTotal { get; set; }
        public List<CarrinhoItem> Items { get; set; }

    }
}