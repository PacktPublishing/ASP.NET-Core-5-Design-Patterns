namespace DomainLayer.Anemic
{
    public class Product : IProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int QuantityInStock { get; set; }
    }
}
