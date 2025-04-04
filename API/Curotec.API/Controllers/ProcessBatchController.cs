using Curotec.API.ActionFilters;
using Curotec.Models.Dtos;
using Curotec.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Curotec.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProcessBatchController : ControllerBase
    {
        private readonly IProcessBatchService _processBatchService;
        public ProcessBatchController(IProcessBatchService processBatchService)
        {
            _processBatchService = processBatchService;
        }


        [HttpPost]
        [SwaggerOperation(
                    Summary = "Insert a list of products according to the batch size",
                    Description = "Insert a list of products according to the batch size",
                    Tags = new[] { "Insert" },
                    OperationId = "insert"
                    )]
        [SwaggerResponse(201, "Insert a new product", typeof(ProductDto))]
        [SwaggerResponse(400, "Returns an 400 error", typeof(ErrorResponse))]
        [SwaggerResponse(404, "Returns an 404 error", typeof(ErrorResponse))]
        [ValidateBatch]
        public async Task<IActionResult> ProcessBatch([FromBody] ProductBatchDto productBatchDto, CancellationToken cancellationToken = default)
        {
            try
            {
                await _processBatchService.ProcessProductBatchAsync(productBatchDto, cancellationToken);
                return Ok("Batch processing started.");
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Processing was canceled.");
            }
        }

    }
}