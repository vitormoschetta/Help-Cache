namespace CacheMemoria.Models
{
    public class Product
    {
        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
        public Product() { }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}