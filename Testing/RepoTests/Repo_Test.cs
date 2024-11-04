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
           
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new DataContext(options);

            
            _loggerMock = new Mock<ILogger<Repo>>();

            _repo = new Repo(_context, _loggerMock.Object);
        }

        //Test för att kontroller att en produkt skapas korrekt samt att statusmeddelande 200 OK returneras
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

        // Test för att kontrollera att en exsisterande produkt returneras när sökning efter den görs, samt att statusmeddelande 200 OK skickas med 
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

        //Test för att kontrollera att en icke exsisterande produkt inte returneras när sökning efter den görs, samt att statusmeddelande 404 NotFound skickas med
        [Fact]
        public async Task GetOneAsync_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Act
            var result = await _repo.GetOneAsync(p => p.Title == "Non-Existent Product");

            // Assert
            Assert.Equal(StatusCode.NOT_FOUND, result.StatusCode);
        }

        // test för att kontrollera att en lista av samtliga produkter returneras genom GetAllAsync samt att statusmeddelande 200 Ok medföljer
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

        //test för att kontrollera att det returneras ett bool värde(TRUE) om en produkt existerar 
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


        //test för att kontrollera att statusmeddelande 404 NotFound returneras ifall en proukt som söks efter inte exsisterar 
        [Fact]
        public async Task ExistsAsync_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Act
            var result = await _repo.ExistsAsync(p => p.Title == "Non-Existent Product");

            // Assert
            Assert.Equal(StatusCode.NOT_FOUND, result.StatusCode);
           
        }

        //test för att kontorllera att en produkt som ska uppdateras gör så korrekt samt att nya uppgifter sparas och sedan ska skatusmeddelande 200 Ok

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

        // test för att kontrollera att statusmeddelande 404 NOtFound returneras om man försöker updatera produkt som inte exstiterar 
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

        // test för att kontrollera att en exsisterande produkt som man försöker ta bort raderas korrekt från databasen om den hittas
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


        //Kontrollerar att statusmeddelande 404 NotFound om man förslker ta bort en produkt som inte exsisterar i databasen 
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
