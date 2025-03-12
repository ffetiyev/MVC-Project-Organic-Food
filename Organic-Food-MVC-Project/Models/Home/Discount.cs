namespace Organic_Food_MVC_Project.Models.Product
{
    public class Discount:BaseEntity
    {
        public int Percent { get; set; }
        public ICollection<Product> Products { get; set; }

    }
}
