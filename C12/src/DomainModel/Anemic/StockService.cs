using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Anemic
{
    public class StockService : IStockService
    {
        public void AddStock(int productId, int amount)
        {
            var product = GetProductById(productId);
            product.QuantityInStock += amount;
            SaveProduct(product);
        }

        public void RemoveStock(int productId, int amount)
        {
            var product = GetProductById(productId);
            if (amount > product.QuantityInStock)
            {
                throw new NotEnoughStockException(product.QuantityInStock, amount);
            }
            product.QuantityInStock -= amount;
            SaveProduct(product);
        }

        #region Should not be in this class
        private Product GetProductById(int productId)
        {
            throw new NotImplementedException();
        }

        private void SaveProduct(Product product)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
