using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Organic_Food_MVC_Project.Services.Interfaces;
using Organic_Food_MVC_Project.ViewModels.Home;

namespace Organic_Food_MVC_Project.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        private readonly ISettingService _settingService;
        private readonly IHttpContextAccessor _accessor;
        public HeaderViewComponent(ISettingService settingService, 
                                   IHttpContextAccessor accessor)
        {
            _settingService = settingService;
            _accessor = accessor;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<BasketVM> basketDatas = new();
            if (_accessor.HttpContext.Request.Cookies["basket"] != null)
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["basket"]);
            }

            int basketCount = basketDatas.Sum(m=>m.ProductCount);


            var datas = await _settingService.GetAllAsync();

            return View(new HeaderVM {Settings=datas,BasketProductCount=basketCount});
        }
    }
}
