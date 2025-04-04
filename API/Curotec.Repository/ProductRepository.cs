using Curotec.Data;
using Curotec.Models.Dtos;
using Curotec.Models.Entities;
using Curotec.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Curotec.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var productDtos = await (from product in _context.Products.AsNoTracking()
                                     select new ProductDto(
                                         product.Name,
                                         product.Price
                                     )).ToListAsync();


            return productDtos;
        }
        public async Task<Product> GetByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {id} was not found.");
            }
            return product;
        }
        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(IEnumerable<Product> products)
        {
            _context.Products.AddRange(products);
            await _context.SaveChangesAsync();
        }
    }
}