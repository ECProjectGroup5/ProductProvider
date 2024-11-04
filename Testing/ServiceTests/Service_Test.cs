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

namespace Infrastructure.Tests.Services
{
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
            // Arrange: Förbered produktmodellen och mocka repo-svaret
            var productModel = new ProductModel { Title = "Test Product", Price = 10, DiscountPrice = 8 };
            var productEntity = new ProductEntity { Title = "Test Product", Price = 10, DiscountPrice = 8 };
            _repoMock.Setup(repo => repo.CreateAsync(It.IsAny<ProductEntity>()))
                     .ReturnsAsync(ResponseFactory.Ok(productEntity));

            // Act: Anropa CreateAsync-metoden
            var result = await _productService.CreateAsync(productModel);

            // Assert: Verifiera resultatstatus och produktinnehåll
            Assert.Equal(StatusCode.OK, result.StatusCode);
            Assert.NotNull(result.ContentResult);
            Assert.Equal("Test Product", ((ProductEntity)result.ContentResult!).Title);
        }

        // Test som kollar av om en produkt inte skapas för att priset saknas
        [Fact]
        public async Task CreateAsync_ShouldNotCreateProductIfPriceIsMissing()
        {
            // Arrange: Förbered en produktmodell utan pris
            var productModel = new ProductModel { Title = "Test Product", DiscountPrice = 8 };
            _repoMock.Setup(repo => repo.CreateAsync(It.IsAny<ProductEntity>()))
                     .ReturnsAsync(ResponseFactory.Error());

            // Act: Anropa CreateAsync-metoden
            var result = await _productService.CreateAsync(productModel);

            // Assert: Verifiera att en felstatus returneras
            Assert.Equal(StatusCode.ERROR, result.StatusCode);
        }

        // Test som kollar av om en produkt inte skapas för att titeln saknas
        [Fact]
        public async Task CreateAsync_ShouldNotCreateProductIfTitleIsMissing()
        {
            // Arrange: Förbered en produktmodell utan titel
            var productModel = new ProductModel { Price = 10, DiscountPrice = 8 };
            _repoMock.Setup(repo => repo.CreateAsync(It.IsAny<ProductEntity>()))
                     .ReturnsAsync(ResponseFactory.Error());

            // Act: Anropa CreateAsync-metoden
            var result = await _productService.CreateAsync(productModel);

            // Assert: Verifiera att en felstatus returneras
            Assert.Equal(StatusCode.ERROR, result.StatusCode);
        }

        // Test som kollar av om en produkt hämtas korrekt om den existerar
        [Fact]
        public async Task GetOneAsync_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange: Förbered en produktentity och mocka repo-svaret
            var articleNumber = Guid.NewGuid();
            var productEntity = new ProductEntity { ArticleNumber = articleNumber, Title = "Existing Product" };
            _repoMock.Setup(repo => repo.GetOneAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>()))
                     .ReturnsAsync(ResponseFactory.Ok(productEntity));

            // Act: Anropa GetOneAsync-metoden
            var result = await _productService.GetOneAsync(articleNumber);

            // Assert: Verifiera resultatstatus och produktinnehåll
            Assert.Equal(StatusCode.OK, result.StatusCode);
            Assert.NotNull(result.ContentResult);
            Assert.Equal("Existing Product", ((ProductEntity)result.ContentResult!).Title);
        }

        // Test som kollar av om en produkt inte existerar och då returnerar en statuskod för NotFound
        [Fact]
        public async Task GetOneAsync_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange: Mocka repo för att returnera NotFound
            _repoMock.Setup(repo => repo.GetOneAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>()))
                     .ReturnsAsync(ResponseFactory.NotFound());

            // Act: Anropa GetOneAsync-metoden
            var result = await _productService.GetOneAsync(Guid.NewGuid());

            // Assert: Verifiera att en statuskod för NotFound returneras
            Assert.Equal(StatusCode.NOT_FOUND, result.StatusCode);
        }

        // Test som kollar av om alla produkter hämtas korrekt
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            // Arrange: Förbered en lista med produkter och mocka repo-svaret
            var products = new List<ProductEntity>
            {
                new ProductEntity { Title = "Product 1" },
                new ProductEntity { Title = "Product 2" }
            };
            _repoMock.Setup(repo => repo.GetAllAsync())
                     .ReturnsAsync(ResponseFactory.Ok(products));

            // Act: Anropa GetAllAsync-metoden
            var result = await _productService.GetAllAsync();

            // Assert: Verifiera resultatstatus och att produkter returneras
            Assert.Equal(StatusCode.OK, result.StatusCode);
            Assert.NotEmpty((IEnumerable<ProductEntity>)result.ContentResult!);
        }

        // Test som kollar av om en produkt finns och sedan så att den uppdateras korrekt
        [Fact]
        public async Task UpdateAsync_ShouldUpdateProduct_WhenProductExists()
        {
            // Arrange: Förbered en produktentity och mocka repo-svaret
            var articleNumber = Guid.NewGuid();
            var productEntity = new ProductEntity { ArticleNumber = articleNumber, Title = "Updated Product" };
            _repoMock.Setup(repo => repo.UpdateAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>(), It.IsAny<ProductEntity>()))
                     .ReturnsAsync(ResponseFactory.Ok(productEntity));

            // Act: Anropa UpdateAsync-metoden
            var result = await _productService.UpdateAsync(articleNumber, productEntity);

            // Assert: Verifiera resultatstatus och uppdaterat produktinnehåll
            Assert.Equal(StatusCode.OK, result.StatusCode);
            Assert.Equal("Updated Product", ((ProductEntity)result.ContentResult!).Title);
        }

        // Test som kollar av om en produkt inte existerar vid uppdatering och då returnerar en statuskod för NotFound
        [Fact]
        public async Task UpdateAsync_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange: Mocka repo för att returnera NotFound vid uppdatering
            _repoMock.Setup(repo => repo.UpdateAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>(), It.IsAny<ProductEntity>()))
                     .ReturnsAsync(ResponseFactory.NotFound());

            // Act: Anropa UpdateAsync-metoden
            var result = await _productService.UpdateAsync(Guid.NewGuid(), new ProductEntity());

            // Assert: Verifiera att en statuskod för NotFound returneras
            Assert.Equal(StatusCode.NOT_FOUND, result.StatusCode);
        }

        // Test som kollar av om en produkt tas bort korrekt
        [Fact]
        public async Task DeleteAsync_ShouldDeleteProduct_WhenProductExists()
        {
            // Arrange: Mocka repo för att returnera Ok vid borttagning
            _repoMock.Setup(repo => repo.DeleteAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>()))
                     .ReturnsAsync(ResponseFactory.Ok());

            // Act: Anropa DeleteAsync-metoden
            var result = await _productService.DeleteAsync(Guid.NewGuid());

            // Assert: Verifiera resultatstatus
            Assert.Equal(StatusCode.OK, result.StatusCode);
        }

        // Test som kollar av om en produkt inte existerar vid borttagning och då returnerar en statuskod för NotFound
        [Fact]
        public async Task DeleteAsync_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange: Mocka repo för att returnera NotFound vid borttagning
            _repoMock.Setup(repo => repo.DeleteAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>()))
                     .ReturnsAsync(ResponseFactory.NotFound());

            // Act: Anropa DeleteAsync-metoden
            var result = await _productService.DeleteAsync(Guid.NewGuid());

            // Assert: Verifiera att en statuskod för NotFound returneras
            Assert.Equal(StatusCode.NOT_FOUND, result.StatusCode);
        }
    }
}
