using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _db;
        public ProductRepository(ProductContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<IEnumerable<Product>> AllAsync()
        {
            var products = await _db.Products.ToArrayAsync();
            return products;
        }

        public async Task DeleteByIdAsync(int productId)
        {
            var product = await _db.Products.FindAsync(productId);
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
        }

        public async Task<Product> FindByIdAsync(int productId)
        {
            var product = await _db.Products.FindAsync(productId);
            return product;
        }

        public async Task InsertAsync(Product product)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _db.Entry(product).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }
    }
}
