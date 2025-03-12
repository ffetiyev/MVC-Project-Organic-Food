using Organic_Food_MVC_Project.Models.Product;

namespace Organic_Food_MVC_Project.ViewModels.Home
{
    public class DiscountVM
    {
        public int Percent { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
