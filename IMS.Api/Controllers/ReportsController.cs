using IMS.Application.Features.Reports.Queries.StockMovements;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("stock-movements")]
        public async Task<IActionResult> StockMovements(
            [FromQuery] DateTime fromUtc,
            [FromQuery] DateTime toUtc,
            [FromQuery] int? warehouseId,
            [FromQuery] int? productId,
            CancellationToken ct)
        {
            // 1) Create the query object with the provided parameters
            var query = new GetStockMovementsQuery(fromUtc, toUtc, warehouseId, productId);

            // 2) Send the query to the mediator and await the result
            var result = await _mediator.Send(query, ct);

            // 3) Check if the result indicates success or failure and return the appropriate HTTP response
            if (!result.IsSuccess)
                return BadRequest(new { error = result.Error });

            return Ok(result.Value);
        }
    }
}
