namespace Organic_Food_MVC_Project.ViewModels.Home
{
    public class BasketDetailVM
    {
        public Dictionary<ProductVM,int> Products { get; set; }
        public decimal Total { get; set; }
    }
}
