﻿using System;
using System.Collections.Generic;
using System.Linq;
using NSE.Core.DomainObjects;
using NSE.Pedidos.Domain.Vouchers;

namespace NSE.Pedidos.Domain.Pedidos
{
    public class Pedido : Entity, IAggregateRoot
    {
        public Pedido(Guid clientId, decimal valorTotal, List<PedidoItem> pedidoItems,
            bool voucherUtilizado = false, decimal desconto = 0, Guid? voucherId = null)
        {
            ClientId = clientId;
            ValorTotal = valorTotal;
            _pedidoItems = pedidoItems;

            Desconto = desconto;
            VoucherUtilizado = voucherUtilizado;
            VoucherId = voucherId;
        }

        private Pedido(){}
       
        public int Codigo { get; private set; }
        public Guid ClientId { get; private set; }
        public Guid? VoucherId { get; private set; }
        public bool VoucherUtilizado { get; private set; }
        public decimal Desconto { get; private set; }
        public decimal ValorTotal { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public PedidoStatus PedidoStatus { get; private set; }

        private readonly List<PedidoItem> _pedidoItems;
        public IReadOnlyCollection<PedidoItem> PedidoItems => _pedidoItems;
        public Endereco Endereco { get; private set; }
        public Voucher Voucher { get; private set; }

        public void AutorizarPedido()
        {
            PedidoStatus = PedidoStatus.Autorizado;
        }

        public void AtribuirVoucher(Voucher voucher)
        {
            VoucherUtilizado = true;
            VoucherId = VoucherId;
            Voucher = voucher;
        }

        public void AtribuirEndereco(Endereco endereco)
        {
            Endereco = endereco;
        }

        public void CalcularValorPedido()
        {
            ValorTotal = PedidoItems.Sum(p => p.CalcularValor());
            CalcularValorTotalDesconto();
        }

        private void CalcularValorTotalDesconto()
        {
            decimal desconto = 0;
            var valor = ValorTotal;

            if (Voucher.TipoDesconto == TipoDescontoVoucher.Porcentagem)
            {
                if (Voucher.Percentual.HasValue)
                {
                    desconto = (valor * Voucher.Percentual.Value) / 100;
                    valor -= desconto;
                }
            }
            else
            {
                if (Voucher.ValorDesconto.HasValue)
                {
                    desconto = Voucher.ValorDesconto.Value;
                    valor -= desconto;
                }
            }

            ValorTotal = valor < 0 ? 0 : valor;
            Desconto = desconto;
        }
    }
}