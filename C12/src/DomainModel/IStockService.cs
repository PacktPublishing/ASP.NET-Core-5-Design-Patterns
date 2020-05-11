namespace DomainModel
{
    public interface IStockService
    {
        void AddStock(int productId, int amount);
        void RemoveStock(int productId, int amount);
    }
}