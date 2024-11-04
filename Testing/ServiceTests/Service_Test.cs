using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Infrastructure.Tests.Services;

public class ProductServiceTests
{
    private readonly ProductService _productService;
    private readonly Mock<IRepo> _repoMock;
    private readonly Mock<ILogger<ProductService>> _loggerMock;

    public ProductServiceTests()
    {
        _repoMock = new Mock<IRepo>();
        _loggerMock = new Mock<ILogger<ProductService>>();
        _productService = new ProductService(_repoMock.Object, _loggerMock.Object);
    }

    // Test som kollar av om en produkt skapas korrekt
    [Fact]
    public async Task CreateAsync_ShouldCreateProduct()
    {
        // Arrange
        var productModel = new ProductModel { Title = "Test Product", Price = 10, DiscountPrice = 8 };
        var productEntity = new ProductEntity { Title = "Test Product", Price = 10, DiscountPrice = 8 };
        _repoMock.Setup(repo => repo.CreateAsync(It.IsAny<ProductEntity>()))
                 .ReturnsAsync(ResponseFactory.Ok(productEntity));

        // Act
        var result = await _productService.CreateAsync(productModel);

        // Assert
        Assert.Equal(StatusCode.OK, result.StatusCode);
        Assert.NotNull(result.ContentResult);
        Assert.Equal("Test Product", ((ProductEntity)result.ContentResult!).Title);
    }

    // Test som kollar av om en produkt inte skapas för att priset saknas
    [Fact]
    public async Task CreateAsync_ShouldNotCreateProductIfPriceIsMissing()
    {
        // Arrange
        var productModel = new ProductModel { Title = "Test Product", DiscountPrice = 8 };
        _repoMock.Setup(repo => repo.CreateAsync(It.IsAny<ProductEntity>()))
                 .ReturnsAsync(ResponseFactory.Error());

        // Act
        var result = await _productService.CreateAsync(productModel);

        // Assert
        Assert.Equal(StatusCode.ERROR, result.StatusCode);
    }

    // Test som kollar av om en produkt inte skapas för att titeln saknas
    [Fact]
    public async Task CreateAsync_ShouldNotCreateProductIfTitleIsMissing()
    {
        // Arrange
        var productModel = new ProductModel { Price = 10, DiscountPrice = 8 };
        _repoMock.Setup(repo => repo.CreateAsync(It.IsAny<ProductEntity>()))
                 .ReturnsAsync(ResponseFactory.Error());

        // Act
        var result = await _productService.CreateAsync(productModel);

        // Assert
        Assert.Equal(StatusCode.ERROR, result.StatusCode);
    }

    // Test som kollar av om en produkt hämtas korrekt om den existerar
    [Fact]
    public async Task GetOneAsync_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var articleNumber = Guid.NewGuid();
        var productEntity = new ProductEntity { ArticleNumber = articleNumber, Title = "Existing Product" };
        _repoMock.Setup(repo => repo.GetOneAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>()))
                 .ReturnsAsync(ResponseFactory.Ok(productEntity));

        // Act
        var result = await _productService.GetOneAsync(articleNumber);

        // Assert
        Assert.Equal(StatusCode.OK, result.StatusCode);
        Assert.NotNull(result.ContentResult);
        Assert.Equal("Existing Product", ((ProductEntity)result.ContentResult!).Title);
    }

    // Test som kollar av om en produkt inte existerar och då returnerar en statuskod för NotFound
    [Fact]
    public async Task GetOneAsync_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        _repoMock.Setup(repo => repo.GetOneAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>()))
                 .ReturnsAsync(ResponseFactory.NotFound());

        // Act
        var result = await _productService.GetOneAsync(Guid.NewGuid());

        // Assert
        Assert.Equal(StatusCode.NOT_FOUND, result.StatusCode);
    }

    // Test som kollar av om alla produkter hämtas korrekt
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProducts()
    {
        // Arrange
        var products = new List<ProductEntity>
        {
            new ProductEntity { Title = "Product 1" },
            new ProductEntity { Title = "Product 2" }
        };
        _repoMock.Setup(repo => repo.GetAllAsync())
                 .ReturnsAsync(ResponseFactory.Ok(products));

        // Act
        var result = await _productService.GetAllAsync();

        // Assert
        Assert.Equal(StatusCode.OK, result.StatusCode);
        Assert.NotEmpty((IEnumerable<ProductEntity>)result.ContentResult!);
    }

    // Test som kollar av om en produkt finns och sedan så att den uppdateras korrekt
    [Fact]
    public async Task UpdateAsync_ShouldUpdateProduct_WhenProductExists()
    {
        // Arrange
        var articleNumber = Guid.NewGuid();
        var productEntity = new ProductEntity { ArticleNumber = articleNumber, Title = "Updated Product" };
        _repoMock.Setup(repo => repo.UpdateAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>(), It.IsAny<ProductEntity>()))
                 .ReturnsAsync(ResponseFactory.Ok(productEntity));

        // Act
        var result = await _productService.UpdateAsync(articleNumber, productEntity);

        // Assert
        Assert.Equal(StatusCode.OK, result.StatusCode);
        Assert.Equal("Updated Product", ((ProductEntity)result.ContentResult!).Title);
    }

    // Test som kollar av om en produkt inte existerar vid uppdatering och då returnerar en statuskod för NotFound
    [Fact]
    public async Task UpdateAsync_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        _repoMock.Setup(repo => repo.UpdateAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>(), It.IsAny<ProductEntity>()))
                 .ReturnsAsync(ResponseFactory.NotFound());

        // Act
        var result = await _productService.UpdateAsync(Guid.NewGuid(), new ProductEntity());

        // Assert
        Assert.Equal(StatusCode.NOT_FOUND, result.StatusCode);
    }

    // Test som kollar av om en produkt tas bort korrekt
    [Fact]
    public async Task DeleteAsync_ShouldDeleteProduct_WhenProductExists()
    {
        // Arrange
        _repoMock.Setup(repo => repo.DeleteAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>()))
                 .ReturnsAsync(ResponseFactory.Ok());

        // Act
        var result = await _productService.DeleteAsync(Guid.NewGuid());

        // Assert
        Assert.Equal(StatusCode.OK, result.StatusCode);
    }

    // Test som kollar av om en produkt inte existerar vid borttagning och då returnerar en statuskod för NotFound
    [Fact]
    public async Task DeleteAsync_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        _repoMock.Setup(repo => repo.DeleteAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>()))
                 .ReturnsAsync(ResponseFactory.NotFound());

        // Act
        var result = await _productService.DeleteAsync(Guid.NewGuid());

        // Assert
        Assert.Equal(StatusCode.NOT_FOUND, result.StatusCode);
    }
}