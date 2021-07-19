using System.Collections.Generic;

namespace NSE.Bff.Compras.Models
{
    public class CarrinhoDto
    {
        public decimal ValorTotal { get; set; }
        public VoucherDTO Voucher { get; set; }
        public bool VoucherUtilizado { get; set; }
        public decimal Desconto { get; set; }
        public List<ItemCarrinhoDto> Itens { get; set; }

        public CarrinhoDto()
        {
            Itens = new List<ItemCarrinhoDto>();
        }
    }
}