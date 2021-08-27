using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Extensions
{
    public class PaginacaoViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke(IPagedList model)
        {
            return View(model);
        }
    }
}
