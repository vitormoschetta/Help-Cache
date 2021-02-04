namespace App01.Models
{
    public class OrderProduct
    {
        public OrderProduct(int orderId, int productId)
        {
            OrderId = orderId;
            ProductId = productId;
        }
        public OrderProduct() { }

        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }     
    }
}