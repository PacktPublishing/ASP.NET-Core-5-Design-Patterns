using Core.Entities;
using Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.UseCases
{
    public class RemoveStocks
    {
        public class Command : IRequest<Product>
        {
            public int ProductId { get; set; }
            public int Amount { get; set; }
        }

        public class Handler : IRequestHandler<Command, Product>
        {
            private readonly IProductRepository _productRepository;
            public Handler(IProductRepository productRepository)
            {
                _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            }

            public Task<Product> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = _productRepository.FindById(request.ProductId);
                if (request.Amount > product.QuantityInStock)
                {
                    throw new NotEnoughStockException(product.QuantityInStock, request.Amount);
                }
                product.QuantityInStock -= request.Amount;
                _productRepository.Update(product);
                return Task.FromResult(product);
            }
        }
    }
}
