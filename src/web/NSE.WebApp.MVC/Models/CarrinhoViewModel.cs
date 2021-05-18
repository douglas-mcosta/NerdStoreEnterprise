using System.Collections.Generic;

namespace NSE.WebApp.MVC.Models
{
    public class CarrinhoViewModel
    {
        public decimal ValorTotal { get; set; }
        public List<ItemCarrinhoViewModel> Itens { get; set; }

        public CarrinhoViewModel()
        {
            Itens = new List<ItemCarrinhoViewModel>();
        }
    }
}