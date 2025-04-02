using AutoMapper;
using Curotec.Models.Dtos;
using Curotec.Models.Entities;
using Curotec.Repository.Interfaces;
using Curotec.Services.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;
using ValidationException = FluentValidation.ValidationException;

namespace Curotec.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IValidator<ProductDto> _validator;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository repository, IValidator<ProductDto> validator, IMapper mapper, ILogger<ProductService> logger)
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> CreateAsync(ProductDto productDto)
        {
            var validationResult = await _validator.ValidateAsync(productDto);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);
            var product = _mapper.Map<Product>(productDto);
            await _repository.AddAsync(product);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> UpdateAsync(int id, ProductDto productDto)
        {
            var existingProduct = await _repository.GetByIdAsync(id);
            if (existingProduct == null) return false;
            var validationResult = await _validator.ValidateAsync(productDto);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);
            _mapper.Map(productDto, existingProduct);
            await _repository.UpdateAsync(existingProduct);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingProduct = await _repository.GetByIdAsync(id);
            if (existingProduct == null) return false;
            await _repository.DeleteAsync(existingProduct);
            return true;
        }
    }
}