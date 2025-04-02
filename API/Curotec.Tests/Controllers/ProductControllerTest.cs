using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Curotec.API.Controllers;
using Curotec.Models.Dtos;
using Curotec.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Curotec.Tests.Controllers
{


    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _serviceMock;
        private readonly Mock<ILogger<ProductsController>> _loggerMock;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _serviceMock = new Mock<IProductService>();
            _loggerMock = new Mock<ILogger<ProductsController>>();
            _controller = new ProductsController(_serviceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Get_ShouldReturnOkResult_WhenProductsExist()
        {
            // Arrange
            var products = new List<ProductDto>
        {
            new ProductDto("Product1", 100),
            new ProductDto("Product2", 150)
        };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(products);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count());
        }

        [Fact]
        public async Task Get_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((ProductDto)null);

            // Act
            var result = await _controller.Get(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Post_ShouldReturnCreatedAtAction_WhenProductIsCreated()
        {
            // Arrange
            var newProductDto = new ProductDto("New Product", 200);
            var createdProductDto = new ProductDto("New Product", 200);

            _serviceMock.Setup(s => s.CreateAsync(newProductDto)).ReturnsAsync(createdProductDto);

            // Act
            var result = await _controller.Post(newProductDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("Get", createdResult.ActionName);
            Assert.IsType<ProductDto>(createdResult.Value);
        }

        [Fact]
        public async Task Post_ShouldReturnBadRequest_WhenValidationFails()
        {
            // Arrange
            var invalidProductDto = new ProductDto("", -10); // Invalid product

            // Mocking service to throw ValidationException
            _serviceMock.Setup(s => s.CreateAsync(invalidProductDto))
                        .ThrowsAsync(new ValidationException("Validation failed"));

            // Act
            var result = await _controller.Post(invalidProductDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenProductIsDeleted()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeleteAsync(It.IsAny<int>())).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeleteAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }

}