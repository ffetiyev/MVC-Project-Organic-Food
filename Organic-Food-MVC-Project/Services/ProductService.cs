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
            var products = await _context.Products.Include(m=>m.ProductImages).Include(m=>m.Discounts).Select(m=>new ProductVM
            {
                Id = m.Id,
                Description = m.Description,
                Name = m.Name,
                Price = m.Price,
                ProductCategoryId=m.ProductCategoryId,
                Discounts = m.Discounts,
                ProductImages=m.ProductImages.Select(m=>new ProductImageVM
                {
                    Name = m.Name,
                    IsMain = m.IsMain,
                }).ToList(),
            }).ToListAsync();

            return products;
        }
    }
}
