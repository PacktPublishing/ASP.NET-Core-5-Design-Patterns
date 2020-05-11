using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Rich
{
    public class StockService : IStockService
    {
        public void AddStock(int productId, int amount)
        {
            var product = GetProductById(productId);
            product.AddStock(amount);
            SaveProduct(product);
        }

        public void RemoveStock(int productId, int amount)
        {
            var product = GetProductById(productId);
            product.RemoveStock(amount);
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
