using Curotec.Models.Dtos;
using Curotec.Models.Entities;

namespace Curotec.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task AddAsync(IEnumerable<Product> products);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
    }
}