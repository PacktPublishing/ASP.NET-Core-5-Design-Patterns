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
        private readonly IProductRepository _repository;
        public StockService(IProductRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public IProduct AddStock(int productId, int amount)
        {
            var data = _repository.FindById(productId);
            var product = new Product
            {
                Id = data.Id,
                Name = data.Name,
                QuantityInStock = data.QuantityInStock
            };
            product.AddStock(amount);
            data.QuantityInStock = product.QuantityInStock;
            _repository.Update(data);
            return product;
        }

        public IProduct RemoveStock(int productId, int amount)
        {
            var data = _repository.FindById(productId);
            var product = new Product
            {
                Id = data.Id,
                Name = data.Name,
                QuantityInStock = data.QuantityInStock
            };
            product.RemoveStock(amount);
            data.QuantityInStock = product.QuantityInStock;
            _repository.Update(data);
            return product;
        }
    }
}
