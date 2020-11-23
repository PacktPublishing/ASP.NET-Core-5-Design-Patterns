using Core;
using Core.UseCases;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [ApiController]
    [Route("products/{productId}/")]
    public class StocksController : ControllerBase
    {
        private readonly IMediator _mediator;
        public StocksController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("add-stocks")]
        public async Task<ActionResult<StockLevel>> AddAsync(
            int productId,
            [FromBody] AddStocks.Command command
        )
        {
            command.ProductId = productId;
            var product = await _mediator.Send(command);
            var stockLevel = new StockLevel(product.QuantityInStock);
            return Ok(stockLevel);
        }

        [HttpPost("remove-stocks")]
        public async Task<ActionResult<StockLevel>> RemoveAsync(
            int productId,
            [FromBody] RemoveStocks.Command command
        )
        {
            try
            {
                command.ProductId = productId;
                var product = await _mediator.Send(command);
                var stockLevel = new StockLevel(product.QuantityInStock);
                return Ok(stockLevel);
            }
            catch (NotEnoughStockException ex)
            {
                return Conflict(new
                {
                    ex.Message,
                    ex.AmountToRemove,
                    ex.QuantityInStock
                });
            }
        }

        public record StockLevel(int QuantityInStock);
    }
}
