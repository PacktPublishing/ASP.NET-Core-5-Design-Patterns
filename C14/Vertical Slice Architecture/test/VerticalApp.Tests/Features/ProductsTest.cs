using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerticalApp.Data;
using VerticalApp.Models;
using Xunit;

namespace VerticalApp.Features.Products
{
    public class ProductsTest
    {
        private readonly IServiceCollection _services;
        public ProductsTest()
        {
            _services = new ServiceCollection();
            var startup = new Startup();
            startup.ConfigureServices(_services);
        }

        public class ListAllProductsTest : ProductsTest
        {
            [Fact]
            public async Task Should_return_all_products()
            {
                // Arrange
                var serviceProvider = _services.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var db = scope.ServiceProvider.GetRequiredService<ProductContext>();
                Seed(db);

                // Act
                var result = await mediator.Send(new ListAllProducts.Command());

                // Assert
                Assert.Collection(result,
                    product => Assert.Equal("Banana", product.Name),
                    product => Assert.Equal("Scotch Bottle", product.Name),
                    product => Assert.Equal("Habanero Pepper", product.Name)
                );

                static void Seed(ProductContext db)
                {
                    db.Products.Add(new Product
                    {
                        Id = 1,
                        Name = "Banana",
                        QuantityInStock = 50
                    });
                    db.Products.Add(new Product
                    {
                        Id = 2,
                        Name = "Scotch Bottle",
                        QuantityInStock = 20
                    });
                    db.Products.Add(new Product
                    {
                        Id = 3,
                        Name = "Habanero Pepper",
                        QuantityInStock = 10
                    });
                    db.SaveChanges();
                }
            }

        }
    }
}
