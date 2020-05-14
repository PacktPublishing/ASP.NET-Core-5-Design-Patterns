namespace DomainLayer
{
    public interface IProduct
    {
        int Id { get; }
        string Name { get; }
        int QuantityInStock { get; }
    }
}
