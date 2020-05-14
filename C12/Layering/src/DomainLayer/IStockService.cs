namespace DomainLayer
{
    public interface IStockService
    {
        IProduct AddStock(int productId, int amount);
        IProduct RemoveStock(int productId, int amount);
    }
}