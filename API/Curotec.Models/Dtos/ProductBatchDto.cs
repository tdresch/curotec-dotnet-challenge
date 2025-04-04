namespace Curotec.Models.Dtos
{
    public record ProductBatchDto(List<ProductDto> Products, int BatchSize);
}