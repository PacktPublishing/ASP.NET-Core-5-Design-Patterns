using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> AllAsync();
        Task<Product> FindByIdAsync(int productId);
        Task UpdateAsync(Product product);
        Task InsertAsync(Product product);
        Task DeleteByIdAsync(int productId);
    }
}
