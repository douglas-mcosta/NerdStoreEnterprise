using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Controllers
{
    [Authorize]
    public class ClienteController : MainController
    {
        public readonly IClienteService _clienteService;


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NovoEndereco(EnderecoViewModel endereco)
        {

            var response = await _clienteService.AdicionarEndereco(endereco);

            if (ResponsePossuiErros(response))
                TempData["Erros"] = ModelState.Values.SelectMany(vm => vm.Errors.Select(e => e.ErrorMessage)).ToList();

            return RedirectToAction("EnderecoEntrega", "Pedido");
        }
    }
}
