
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Organic_Food_MVC_Project.Data;
using Organic_Food_MVC_Project.Services.Interfaces;
using Organic_Food_MVC_Project.ViewModels.Home;

namespace Organic_Food_MVC_Project.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context)
        {
            _context=context;
        }
        public async Task<IEnumerable<ProductVM>> GetAllAsync()
        {
            var products = await _context.Products.Include(m=>m.ProductImages).Include(m=>m.ProductCategory).Include(m=>m.Discounts).Select(m=>new ProductVM
            {
                Id = m.Id,
                Description = m.Description,
                Name = m.Name,
                Price = m.Price,
                ProductCategoryId=m.ProductCategoryId,
                Discounts = m.Discounts,
                CategoryName=m.ProductCategory.Name,
                ProductImages=m.ProductImages.Select(m=>new ProductImageVM
                {
                    Name = m.Name,
                    IsMain = m.IsMain,
                }).ToList(),
            }).ToListAsync();

            return products;
        }

        public async Task<ProductVM> GetByIdAsync(int? id)
        {
            var product =await _context.Products.Include(m => m.ProductImages).Include(m => m.ProductCategory).Include(m => m.Discounts).FirstOrDefaultAsync(m=>m.Id == id);
            if (product == null) return null;
            return new ProductVM
            {
                Id = product.Id,
                CategoryName=product.ProductCategory.Name,
                Description = product.Description,
                Name = product.Name,
                Price = product.Price,
                ProductImages=product.ProductImages.Select(m=>new ProductImageVM { IsMain = m.IsMain,Name=m.Name}).ToList(),
                Discounts = product.Discounts.ToList(),
               
            };
            
        }
    }
}
