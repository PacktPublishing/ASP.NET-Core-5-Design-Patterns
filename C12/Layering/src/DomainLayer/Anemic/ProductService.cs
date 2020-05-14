using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Anemic
{
    public class ProductService : IProductService
    {
        private readonly ProductContext _db;
        public ProductService(ProductContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public IEnumerable<IProduct> All()
        {
            return _db.Products.Select(p => new Product
            {
                Id = p.Id,
                Name = p.Name,
                QuantityInStock = p.QuantityInStock
            });
        }
    }
}
