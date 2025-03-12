namespace Organic_Food_MVC_Project.ViewModels.Home
{
    public class HomeVM
    {
        public IEnumerable<CategoryVM> Categories { get; set; }
        public IEnumerable<ProductVM> Products { get; set; }
        public IEnumerable<SliderVM> Sliders { get; set; }
        public IEnumerable<ServiceVM> Services { get; set; }
    }
}
