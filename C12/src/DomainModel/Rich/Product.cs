namespace DomainModel.Rich
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int QuantityInStock { get; set; }

        public void AddStock(int amount)
        {
            QuantityInStock += amount;
        }

        public void RemoveStock(int amount)
        {
            if (amount > QuantityInStock)
            {
                throw new NotEnoughStockException(QuantityInStock, amount);
            }
            QuantityInStock -= amount;
        }
    }
}
