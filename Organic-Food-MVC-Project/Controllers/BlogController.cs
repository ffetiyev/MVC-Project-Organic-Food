using Microsoft.AspNetCore.Mvc;
using Organic_Food_MVC_Project.Services.Interfaces;
using Organic_Food_MVC_Project.ViewModels.Blog;

namespace Organic_Food_MVC_Project.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<BlogVM> blogs=await _blogService.GetAllAsync();
            return View(blogs);
        }
    }
}
