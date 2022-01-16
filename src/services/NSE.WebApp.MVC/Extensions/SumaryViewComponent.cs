using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NSE.WebApp.MVC.Extensions
{
    public class SumaryViewComponent: ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}