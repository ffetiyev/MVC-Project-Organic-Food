using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Organic_Food_MVC_Project.Models;
using Organic_Food_MVC_Project.Services;
using Organic_Food_MVC_Project.Services.Interfaces;
using Organic_Food_MVC_Project.ViewModels.Blog;
using Organic_Food_MVC_Project.ViewModels.Home;

namespace Organic_Food_MVC_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductCategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ISliderService _sliderService;
        private readonly IServiceService _serviceService;
        private readonly IAdvertisementService _advertisementService;
        private readonly IAboutService _aboutService;
        private readonly IBrandService _brandService;
        private readonly IBlogService _blogService;
        public HomeController(IProductCategoryService categoryService,
                              IProductService productService,
                              ISliderService sliderService,
                              IServiceService serviceService,
                              IAdvertisementService advertisementService,
                              IAboutService aboutService,
                              IBrandService brandService,
                              IBlogService blogService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _sliderService = sliderService;
            _serviceService = serviceService;
            _advertisementService = advertisementService;
            _aboutService = aboutService;
            _brandService = brandService;
            _blogService = blogService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<CategoryVM> categories = await _categoryService.GetAllAsync();
            IEnumerable<ProductVM> products =await _productService.GetAllAsync();
            IEnumerable<SliderVM> sliders = await _sliderService.GetAllAsync();
            IEnumerable<ServiceVM> services = await _serviceService.GetAllAsync();
            AdvertisementVM advertisement=await _advertisementService.GetAsync();
            AboutVM about= await _aboutService.GetAsync();
            IEnumerable<BrandVM> brands = await _brandService.GetAllAsync();
            IEnumerable<BlogVM> blogs = await _blogService.GetAllAsync();
 
            HomeVM homeVM = new HomeVM()
            {
                Blogs = blogs,
                Brands = brands,
                About = about,
                Advertisement=advertisement,
                Services=services,
                Sliders=sliders,
                Products=products,
                Categories=categories
            };
            return View(homeVM);
        }

    }
}
