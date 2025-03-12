using Microsoft.AspNetCore.Mvc;

namespace Organic_Food_MVC_Project.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
