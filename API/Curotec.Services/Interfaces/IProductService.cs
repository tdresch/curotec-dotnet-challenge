using Curotec.Models.Dtos;

namespace Curotec.Services.Interfaces
{

    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto> GetByIdAsync(int id);
        Task<ProductDto> CreateAsync(ProductDto productDto);
        Task<bool> UpdateAsync(int id, ProductDto productDto);
        Task<bool> DeleteAsync(int id);
    }
}