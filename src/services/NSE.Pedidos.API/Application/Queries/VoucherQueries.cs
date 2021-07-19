﻿using System.Threading.Tasks;
using NSE.Pedidos.API.Application.DTO;
using NSE.Pedidos.Domain.Vouchers;

namespace NSE.Pedidos.API.Application.Queries
{
    public class VoucherQueries : IVoucherQueries
    {
        private IVoucherRepository _voucherRepository;

        public VoucherQueries(IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
        }

        public async Task<VoucherDTO> ObterVoucherPorCodigo(string codigo)
        {
            var voucher = await _voucherRepository.ObterVoucherPorCodigo(codigo);

            if (voucher is null) return null;

            if (!voucher.EstaValidoParaUtilizacao()) return null;

            return new VoucherDTO
            {
                Codigo = voucher.Codigo,
                TipoDesconto = (int)voucher.TipoDesconto,
                Percentual = voucher.Percentual,
                ValorDesconto = voucher.ValorDesconto
            };

        }
    }
}