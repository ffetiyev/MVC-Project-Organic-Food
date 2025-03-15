using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Organic_Food_MVC_Project.Areas.Admin.ViewModels.Product;
using Organic_Food_MVC_Project.Data;
using Organic_Food_MVC_Project.Models.Product;
using Organic_Food_MVC_Project.Services.Interfaces;
using Organic_Food_MVC_Project.ViewModels.Home;

namespace Organic_Food_MVC_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _env;
        public ProductController(AppDbContext context,
                                 IProductService productService,
                                 IWebHostEnvironment env)
        {
            _context = context;
            _productService = productService;
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllAsync();
            List<ProductDetailVM> result = new List<ProductDetailVM>();
            foreach (var product in products)
            {
                result.Add(new ProductDetailVM
                {
                    CategoryName = product.CategoryName,
                    Description = product.Description,
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Images = product.ProductImages.ToList(),
                });
            }
            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            var existProduct = await _productService.GetByIdAsync(id);
           
            return View(new ProductDetailVM
            {
                Discounts=existProduct.Discounts.ToList(),
                CategoryName= existProduct.CategoryName,
                Description= existProduct.Description,
                Id= existProduct.Id,
                Images= existProduct.ProductImages.ToList(),
                Name= existProduct.Name,
                Price= existProduct.Price,
            });
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _context.ProductCategories.Select(m => new SelectListItem
            {
                Text = m.Name,
                Value = m.Name,
            }).ToListAsync();

            ViewBag.Categories = categories;

            var discounts = await _context.Discounts.Select(m => new SelectListItem
            {
                Text = m.Percent.ToString(),
                Value = m.Percent.ToString(),
            }).ToListAsync();

            ViewBag.Discounts = discounts;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM request)
        {
            var categories = await _context.ProductCategories.Select(m => new SelectListItem
            {
                Text = m.Name,
                Value = m.Name,
            }).ToListAsync();

            ViewBag.Categories = categories;

            var discounts = await _context.Discounts.Select(m => new SelectListItem
            {
                Text = m.Percent.ToString(),
                Value = m.Percent.ToString(),
            }).ToListAsync();

            ViewBag.Discounts = discounts;

            if (!ModelState.IsValid) return View(request);

            List<ProductImage> productImages = new List<ProductImage>();

            foreach (var image in request.Images)
            {
                if (!image.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("Images", "File type must be image!");
                    return View(request);
                }
                if (image.Length / 1024 > 1024)
                {
                    ModelState.AddModelError("Images", "Image size must be smaller than 1mb!");
                    return View(request);
                }
            }

            foreach (var image in request.Images)
            {

                string fileName= Guid.NewGuid().ToString() + "-" + image.FileName;
                string filePath = Path.Combine(_env.WebRootPath,"assets","images","products",fileName);
                using(FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                productImages.Add(new ProductImage
                {
                    Name = fileName,
                    IsMain=false,
                });
            }
            productImages.FirstOrDefault().IsMain = true;
            List<Discount> productDiscounts = new List<Discount>();
            productDiscounts.Add(new Discount
            {
                Percent = request.Discount,
            });
            await _context.Products.AddAsync(new Product
            {
                Name=request.Name,
                Description=request.Description,
                Discounts=productDiscounts,
                Price=request.Price,
                ProductCategoryId= _context.ProductCategories.FirstOrDefaultAsync(m => m.Name == request.CategoryName).Id,
                ProductImages = productImages
            });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var existProduct = await _context.Products.Include(m=>m.ProductImages).FirstOrDefaultAsync(m=>m.Id==id);
            if (existProduct == null) return NotFound();

            foreach (var item in existProduct.ProductImages)
            {
                string filePath = Path.Combine(_env.WebRootPath, "assets", "images", "products", item.Name);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            _context.Products.Remove(existProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null) return BadRequest();
            var existProduct = await _context.Products.Include(m => m.ProductImages)
                                                      .Include(m=>m.ProductCategory)
                                                      .Include(m=>m.Discounts)
                                                      .FirstOrDefaultAsync(m => m.Id == id);
            if (existProduct == null) return NotFound();

            var categories = await _context.ProductCategories.Select(m => new SelectListItem
            {
                Text = m.Name,
                Value = m.Name,
            }).ToListAsync();

            ViewBag.Categories = categories;

            var discounts = await _context.Discounts.Select(m => new SelectListItem
            {
                Text = m.Percent.ToString(),
                Value = m.Percent.ToString(),
            }).ToListAsync();

            ViewBag.Discounts = discounts;
            return View(new ProductEditVM
            {
                Id=existProduct.Id,
                Name= existProduct.Name,
                Description= existProduct.Description,
                Price= existProduct.Price,
                CategoryName=existProduct.ProductCategory.Name,
                Discount=existProduct.Discounts.First().Percent,
                ProductImages=existProduct.ProductImages.Select(m=>new ProductImageVM
                {
                    IsMain=m.IsMain,
                    Name=m.Name,
                }).ToList(),
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,ProductEditVM request)
        {
            if (id == null) return BadRequest();
            var existProduct = await _context.Products.Include(m => m.ProductImages)
                                                      .Include(m => m.ProductCategory)
                                                      .Include(m => m.Discounts)
                                                      .FirstOrDefaultAsync(m => m.Id == id);
            if (existProduct == null) return NotFound();

            var categories = await _context.ProductCategories.Select(m => new SelectListItem
            {
                Text = m.Name,
                Value = m.Name,
            }).ToListAsync();

            ViewBag.Categories = categories;

            var discounts = await _context.Discounts.Select(m => new SelectListItem
            {
                Text = m.Percent.ToString(),
                Value = m.Percent.ToString(),
            }).ToListAsync();

            ViewBag.Discounts = discounts;

            if (ModelState.IsValid) return View(request);

            if (request.UploadImages != null)
            {
                foreach (var image in request.UploadImages)
                {
                    if (!image.ContentType.Contains("image/"))
                    {
                        ModelState.AddModelError("Images", "File type must be image!");
                        return View(request);
                    }
                    if (image.Length / 1024 > 1024)
                    {
                        ModelState.AddModelError("Images", "Image size must be smaller than 1mb!");
                        return View(request);
                    }
                }
                List<ProductImage> uploadImages = new List<ProductImage>();
                foreach (var image in request.UploadImages)
                {
                    string fileName = Guid.NewGuid().ToString() + "-" + image.FileName;
                    string filePath = Path.Combine(_env.WebRootPath,"assets", "images", "products", fileName);
                    using(FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                       await image.CopyToAsync(stream);
                    }
                    uploadImages.Add(new ProductImage { IsMain=false,Name=fileName,ProductId=request.Id});
                }
                await _context.ProductImages.AddRangeAsync(uploadImages);
            }
            existProduct.Name=request.Name;
            existProduct.Description=request.Description;
            existProduct.Price=request.Price;
            existProduct.Discounts.Add(new Discount { Id=_context.Discounts.FirstOrDefaultAsync(m=>m.Percent==request.Discount).Id});
            existProduct.ProductCategory=await _context.ProductCategories.FirstOrDefaultAsync(m=>m.Name==request.CategoryName);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
