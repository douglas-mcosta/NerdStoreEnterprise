﻿using System;
using NSE.Core.DomainObjects;

namespace NSE.Pedidos.Domain.Pedidos
{
    public class PedidoItem : Entity
    {
        public Guid PedidoId { get; private set; }
        public Guid ProdutoId { get; private set; }
        public string ProdutoNome { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }
        public string ProdutoImagem { get; private set; }

        //EF
        public Pedido Pedido { get; private set; }

        public PedidoItem(Guid pedidoId, Guid produtoId, string produtoNome, int quantidade, decimal valorUnitario, string produtoImagem)
        {
            PedidoId = pedidoId;
            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
            ProdutoImagem = produtoImagem;
        }

        private PedidoItem() { }

        internal decimal CalcularValor() => Quantidade * ValorUnitario;
    }
}