

using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infrastructure.Tests.Repositories
{
    public class RepoTests
    {
        private readonly Repo _repo;
        private readonly Mock<ILogger<Repo>> _loggerMock;
        private readonly DataContext _context;

        public RepoTests()
        {
            // Set up InMemory database
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new DataContext(options);

            // Mock ILogger
            _loggerMock = new Mock<ILogger<Repo>>();

            // Create instance of Repo with InMemory database and Mock logger
            _repo = new Repo(_context, _loggerMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddProduct()
        {
            // Arrange
            var product = new ProductEntity { Title = "Test Product", Price = 10, DiscountPrice= 10 };

            // Act
            var result = await _repo.CreateAsync(product);

            // Assert
            Assert.Equal(StatusCode.OK, result.StatusCode);
            Assert.Contains(_context.Products, p => p.Title == "Test Product");
        }

        [Fact]
        public async Task GetOneAsync_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var product = new ProductEntity { Title = "Test Product", Price = 10, DiscountPrice = 10 };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetOneAsync(p => p.Title == "Test Product");

            // Assert
            Assert.Equal(StatusCode.OK, result.StatusCode);
            Assert.NotNull((ProductEntity)result.ContentResult!);
            Assert.Equal("Test Product", ((ProductEntity)result.ContentResult!).Title);
        }

        [Fact]
        public async Task GetOneAsync_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Act
            var result = await _repo.GetOneAsync(p => p.Title == "Non-Existent Product");

            // Assert
            Assert.Equal(StatusCode.NOT_FOUND, result.StatusCode);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            // Arrange
            _context.Products.AddRange(
                new ProductEntity { Title = "Test Product1", Price = 10, DiscountPrice = 10 },
                new ProductEntity { Title = "Test Product2", Price = 10, DiscountPrice = 10 });
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetAllAsync();

            // Assert
            Assert.Equal(StatusCode.OK, result.StatusCode);
            Assert.True(((IEnumerable<ProductEntity>)result.ContentResult!).Any());
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnTrue_WhenProductExists()
        {
            // Arrange
            var product = new ProductEntity {Title = "Existing Product", Price = 10, DiscountPrice = 10 };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.ExistsAsync(p => p.Title == "Existing Product");

            // Assert
            Assert.Equal(StatusCode.EXISTS, result.StatusCode);
            Assert.True((bool)result.ContentResult!);
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Act
            var result = await _repo.ExistsAsync(p => p.Title == "Non-Existent Product");

            // Assert
            Assert.Equal(StatusCode.NOT_FOUND, result.StatusCode);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateProduct_WhenProductExists()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var product = new ProductEntity {Title = $"Test Product {guid}", Price = 10, DiscountPrice = 10 };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var updatedProduct = await _context.Products.FirstOrDefaultAsync(p => p.Title == $"Test Product {guid}");
            updatedProduct!.Title = "New Title";

            // Act
            var result = await _repo.UpdateAsync(p => p.Title == $"Test Product {guid}", updatedProduct);
            var entityResult = (ProductEntity)result.ContentResult!;

            // Assert
            Assert.Equal(StatusCode.OK, result.StatusCode);
            Assert.Equal("New Title", entityResult.Title);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var updatedProduct = new ProductEntity {Title = $"Test Product {guid}", Price = 10, DiscountPrice = 10 };

            // Act
            var result = await _repo.UpdateAsync(p => p.Title == $"Test Product {guid}", updatedProduct);

            // Assert
            Assert.Equal(StatusCode.NOT_FOUND, result.StatusCode);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveProduct_WhenProductExists()
        {
            // Arrange
            var product = new ProductEntity { Title = "Test Product", Price = 10, DiscountPrice = 10 };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.DeleteAsync(p => p.Title == "Test Product");

            // Assert
            Assert.Equal(StatusCode.OK, result.StatusCode);
            Assert.DoesNotContain(_context.Products, p => p.Title == "Product to Delete");
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Act
            var result = await _repo.DeleteAsync(p => p.Title == $"Test Product{Guid.NewGuid()}");

            // Assert
            Assert.Equal(StatusCode.NOT_FOUND, result.StatusCode);
        }
    }
}
