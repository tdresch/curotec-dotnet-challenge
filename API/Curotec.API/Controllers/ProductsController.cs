using Curotec.API.ActionFilters;
using Curotec.Models.Dtos;
using Curotec.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Curotec.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService service, ILogger<ProductsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all products",
            Tags = new[] { "Get All" },
            Description = "Get all products",
            OperationId = "get"
            )]
        [SwaggerResponse(200, "Returns a product", typeof(ProductDto))]
        [SwaggerResponse(400, "Returns an 400 error", typeof(ErrorResponse))]
        [SwaggerResponse(404, "Returns an 404 error", typeof(ErrorResponse))]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get a product by ID",
            Tags = new[] { "Get and Search" },
            Description = "Get a product by ID",
            OperationId = "get"
            )]
        [SwaggerResponse(200, "Returns a product", typeof(ProductDto))]
        [SwaggerResponse(400, "Returns an 400 error", typeof(ErrorResponse))]
        [SwaggerResponse(404, "Returns an 404 error", typeof(ErrorResponse))]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _service.GetByIdAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Insert a new product",
            Tags = new[] { "Insert" },
            Description = "Insert a new product",
            OperationId = "insert"
            )]
        [SwaggerResponse(201, "Insert a new product", typeof(ProductDto))]
        [SwaggerResponse(400, "Returns an 400 error", typeof(ErrorResponse))]
        [SwaggerResponse(404, "Returns an 404 error", typeof(ErrorResponse))]
        [ValidateProduct]
        public async Task<IActionResult> Post([FromBody] ProductDto productDto)
        {
            try
            {
                ProductDto product = await _service.CreateAsync(productDto);
                return CreatedAtAction(nameof(Get), new { id = product.Name }, product);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update a product",
            Tags = new[] { "Update" },
            Description = "Update an existing product",
            OperationId = "update"
            )]
        [SwaggerResponse(204, "Update an existing product", typeof(ProductDto))]
        [SwaggerResponse(400, "Returns an 400 error", typeof(ErrorResponse))]
        [SwaggerResponse(404, "Returns an 404 error", typeof(ErrorResponse))]
        [ValidateProduct]
        public async Task<IActionResult> Put(int id, [FromBody] ProductDto productDto)
        {
            var updated = await _service.UpdateAsync(id, productDto);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete a product",
            Tags = new[] { "Delete" },
            Description = "Delete an existing product",
            OperationId = "delete"
            )]
        [SwaggerResponse(204, "Delete an existing product", typeof(ProductDto))]
        [SwaggerResponse(400, "Returns an 400 error", typeof(ErrorResponse))]
        [SwaggerResponse(404, "Returns an 404 error", typeof(ErrorResponse))]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}