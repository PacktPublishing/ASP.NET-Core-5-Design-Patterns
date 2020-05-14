using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Rich
{
    public class StockService : IStockService
    {
        private readonly ProductContext _db;
        public StockService(ProductContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public IProduct AddStock(int productId, int amount)
        {
            var data = _db.Products.Find(productId);
            var product = new Product
            {
                Id = data.Id,
                Name = data.Name,
                QuantityInStock = data.QuantityInStock
            };
            product.AddStock(amount);
            data.QuantityInStock = product.QuantityInStock;
            _db.SaveChanges();
            return product;
        }

        public IProduct RemoveStock(int productId, int amount)
        {
            var data = _db.Products.Find(productId);
            var product = new Product
            {
                Id = data.Id,
                Name = data.Name,
                QuantityInStock = data.QuantityInStock
            };
            product.RemoveStock(amount);
            data.QuantityInStock = product.QuantityInStock;
            _db.SaveChanges();
            return product;
        }
    }
}
