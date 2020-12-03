namespace AnemicDomainLayer
{
    public interface IStockService
    {
        Product AddStock(int productId, int amount);
        Product RemoveStock(int productId, int amount);
    }
}