using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Organic_Food_MVC_Project.Models;
using Organic_Food_MVC_Project.Services;
using Organic_Food_MVC_Project.Services.Interfaces;
using Organic_Food_MVC_Project.ViewModels.Home;

namespace Organic_Food_MVC_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductCategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ISliderService _sliderService;
        private readonly IServiceService _serviceService;
        public HomeController(IProductCategoryService categoryService,
                              IProductService productService,
                              ISliderService sliderService,
                              IServiceService serviceService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _sliderService = sliderService;
            _serviceService = serviceService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<CategoryVM> categories = await _categoryService.GetAllAsync();
            IEnumerable<ProductVM> products =await _productService.GetAllAsync();
            IEnumerable<SliderVM> sliders = await _sliderService.GetAllAsync();
            IEnumerable<ServiceVM> services = await _serviceService.GetAllAsync();
            HomeVM homeVM = new HomeVM()
            {
                Services=services,
                Sliders=sliders,
                Products=products,
                Categories=categories
            };
            return View(homeVM);
        }

    }
}
