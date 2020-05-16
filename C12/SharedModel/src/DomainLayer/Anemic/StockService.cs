﻿using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Anemic
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
            var product = _repository.FindById(productId);
            product.QuantityInStock += amount;
            _repository.Update(product);

            return new Product
            {
                Id = product.Id,
                Name = product.Name,
                QuantityInStock = product.QuantityInStock
            };
        }

        public IProduct RemoveStock(int productId, int amount)
        {
            var product = _repository.FindById(productId);
            if (amount > product.QuantityInStock)
            {
                throw new NotEnoughStockException(product.QuantityInStock, amount);
            }
            product.QuantityInStock -= amount;
            _repository.Update(product);

            return new Product
            {
                Id = product.Id,
                Name = product.Name,
                QuantityInStock = product.QuantityInStock
            };
        }
    }
}
