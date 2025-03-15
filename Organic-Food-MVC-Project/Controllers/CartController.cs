﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Organic_Food_MVC_Project.Services.Interfaces;
using Organic_Food_MVC_Project.ViewModels.Home;

namespace Organic_Food_MVC_Project.Controllers
{
    public class CartController : Controller
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IProductService _productService;
        public CartController(IHttpContextAccessor accessor,
                              IProductService productService)
        {
            _accessor=accessor;
            _productService=productService;
        }
        public async Task<IActionResult> Index()
        {
            List<BasketVM> basketDatas = new();
            if (_accessor.HttpContext.Request.Cookies["basket"] != null)
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["basket"]);
            }

            Dictionary<ProductVM,int> products = new();
            foreach (var item in basketDatas)
            {
                var product =await _productService.GetByIdAsync(item.ProductId);
                products.Add(product,item.ProductCount);
            }
            decimal total = products.Sum(m=>m.Key.Price*m.Value);
            return View(new BasketDetailVM { Products=products,Total=total});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            List<BasketVM> basketDatas = new();
            if (_accessor.HttpContext.Request.Cookies["basket"] != null)
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["basket"]);
            }

            var existProduct = basketDatas.FirstOrDefault(m=>m.ProductId==id);
            basketDatas.Remove(existProduct);

            _accessor.HttpContext.Response.Cookies.Append("basket",JsonConvert.SerializeObject(basketDatas));

            return Ok();
        }
    }
}
