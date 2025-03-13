using Microsoft.AspNetCore.Mvc;
using Organic_Food_MVC_Project.Data;
using Organic_Food_MVC_Project.Services.Interfaces;
using Organic_Food_MVC_Project.ViewModels.Home;
using System.Threading.Tasks;

namespace Organic_Food_MVC_Project.Controllers
{
    public class ProductDetailController : Controller
    {
        private readonly IProductService _productService;
        public ProductDetailController(IProductService productService )
        {
           _productService = productService;
        }
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ProductVM product =await _productService.GetByIdAsync((int)id);
            if (product == null)
            {
                return RedirectToAction("NotFoundException", "Error");
            }

            return View(product);
        }
    }
}
