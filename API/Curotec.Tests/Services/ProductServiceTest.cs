using AutoMapper;
using Curotec.Models.Dtos;
using Curotec.Models.Entities;
using Curotec.Repository.Interfaces;
using Curotec.Services;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;

namespace Curotec.Tests.Services;
public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly Mock<IValidator<ProductDto>> _validatorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<ProductService>> _loggerMock;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _validatorMock = new Mock<IValidator<ProductDto>>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<ProductService>>();
        _service = new ProductService(_repositoryMock.Object, _validatorMock.Object, _mapperMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnListOfProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product(){ Id = 1, Name = "Product1", Price = 100 },
            new Product(){ Id = 2, Name = "Product2", Price = 150 }
        };
        var productDtos = new List<ProductDto>
        {
            new ProductDto("Product1", 100),
            new ProductDto("Product2", 150)
        };

        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);
        _mapperMock.Setup(m => m.Map<IEnumerable<ProductDto>>(It.IsAny<IEnumerable<Product>>())).Returns(productDtos);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal("Product1", result.First().Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var product = new Product() { Id = 1, Name = "Product1", Price = 100 };
        var productDto = new ProductDto("Product1", 100);

        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
        _mapperMock.Setup(m => m.Map<ProductDto>(It.IsAny<Product>())).Returns(productDto);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Product1", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Product)null);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.Null(result);
    }

}
