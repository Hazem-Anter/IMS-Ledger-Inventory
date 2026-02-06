using IMS.Application.Features.Products.Queries.ProductTimeline;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Get the timeline of stock movements for a specific product,
        // with optional filtering by date range and warehouse, and support for pagination.
        // ex --> GET: api/products/{productId}/timeline?fromUtc=2024-01-01T00:00:00Z&toUtc=2024-12-31T23:59:59Z&warehouseId=1&page=1&pageSize=50
        [HttpGet("{productId:int}/timeline")]
        public async Task<IActionResult> Timeline(
        [FromRoute] int productId,
        [FromQuery] DateTime? fromUtc,
        [FromQuery] DateTime? toUtc,
        [FromQuery] int? warehouseId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken ct = default)
        {
            var query = new GetProductTimelineQuery(
                productId,
                fromUtc,
                toUtc,
                warehouseId,
                page,
                pageSize);

            var result = await _mediator.Send(query, ct);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Error });

            return Ok(result.Value);
        }
    }
}
