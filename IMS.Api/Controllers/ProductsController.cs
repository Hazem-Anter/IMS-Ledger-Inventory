using IMS.Api.Common;
using IMS.Api.Contracts.Products;
using IMS.Application.Features.Products.Commands.CreateProduct;
using IMS.Application.Features.Products.Queries.ProductTimeline;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Api.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
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

            var data = result.OkOrThrow();

            return Ok(data);
        }

        // Create a new product with the specified details,
        // including name, SKU, optional barcode, and minimum stock level.
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest req, CancellationToken ct)
        {
            // 1) Validate the incoming request data (e.g., check for required fields, validate formats).
            var id = (await _mediator.Send(
                new CreateProductCommand(req.Name, req.Sku, req.Barcode, req.MinStockLevel), ct))
                .OkOrThrow();

            // 2) return a response containing the ID of the newly created product,
            // along with any relevant metadata or links for further actions (e.g., retrieving the product details).
            return Ok(new CreateProductResponse(id));
        }
    }
}
