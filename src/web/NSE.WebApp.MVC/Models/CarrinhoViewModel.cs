using System.Collections.Generic;

namespace NSE.WebApp.MVC.Models
{
    public class CarrinhoViewModel
    {
        public VoucherViewModel Voucher { get; set; }
        public decimal ValorTotal { get; set; }
        public bool VoucherUtilizado { get; set; }
        public decimal Desconto { get; set; }
        public List<ItemCarrinhoViewModel> Itens { get; set; }

        public CarrinhoViewModel()
        {
            Itens = new List<ItemCarrinhoViewModel>();
        }
    }
}