using Curotec.Models.Dtos;
using Curotec.Models.Entities;
using Curotec.Repository.Interfaces;
using Curotec.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Curotec.Services
{
    public class ProcessBatchService : IProcessBatchService
    {

        private readonly IProductRepository _repository;
        private readonly ILogger<ProcessBatchService> _logger;

        public ProcessBatchService(IProductRepository repository, ILogger<ProcessBatchService> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task ProcessProductBatchAsync(ProductBatchDto productBatchDto, CancellationToken cancellationToken)
        {
            var batchSize = productBatchDto.BatchSize;
            var products = productBatchDto.Products.Select(p => new Product { Name = p.Name, Price = p.Price }).ToList();

            // Process products in batches asynchronously
            var totalProducts = products.Count;
            for (int i = 0; i < totalProducts; i += batchSize)
            {
                var batch = products.Skip(i).Take(batchSize).ToList();

                // Handle cancellation request before continuing
                cancellationToken.ThrowIfCancellationRequested();

                await _repository.AddAsync(batch);
                _logger.LogInformation($"Processed batch {i / batchSize + 1} of {totalProducts / batchSize + 1}");
            }
        }
    }
}