using Curotec.Models.Dtos;

namespace Curotec.Services.Interfaces
{
    public interface IProcessBatchService
    {
        Task ProcessProductBatchAsync(ProductBatchDto productBatchDto, CancellationToken cancellationToken);
    }
}