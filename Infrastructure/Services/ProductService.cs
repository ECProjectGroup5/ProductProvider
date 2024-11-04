using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class ProductService(IRepo repo, ILogger<ProductService> logger)
{
    private readonly IRepo repo = repo;
    private readonly ILogger<ProductService> _logger = logger;

    public async Task<ResponseResult> CreateAsync(ProductModel product)
    {
        try
        {
            var entity = new ProductEntity
            {
                Title = product.Title,
                Ingress = product.Ingress,
                Description = product.Description,
                Price = product.Price,
                DiscountPrice = product.DiscountPrice,
                Manufacturer = product.Manufacturer,
                PrimaryImage = product.PrimaryImage
            };
            
            var result = await repo.CreateAsync(entity);

            if (result.StatusCode == StatusCode.OK)
            {
                return ResponseFactory.Ok((ProductEntity)result.ContentResult!);
            }

            return ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"Error: ProductProvider.ProductService.CreateAsync {ex.Message}");
            return ResponseFactory.Error();
        }
    }

    public async Task<ResponseResult> GetOneAsync(Guid articleNumber)
    {
        try
        {
            var result = await repo.GetOneAsync(x => x.ArticleNumber == articleNumber);

            if (result.StatusCode == StatusCode.OK)
            {
                return ResponseFactory.Ok((ProductEntity)result.ContentResult!);
            }

            return ResponseFactory.NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"Error: ProductProvider.ProductService.GetOneAsync {ex.Message}");
            return ResponseFactory.Error();
        }
    }

    public async Task<ResponseResult> GetAllAsync()
    {
        try
        {
            var result = await repo.GetAllAsync();

            if (result.StatusCode == StatusCode.OK)
            {
                return ResponseFactory.Ok((IEnumerable<ProductEntity>)result.ContentResult!);
            }
            else if (result.StatusCode == StatusCode.NOT_FOUND)
                return ResponseFactory.NotFound();

            return ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"Error: ProductProvider.ProductService.GetAllAsync {ex.Message}");
            return ResponseFactory.Error();
        }
    }

    public async Task<ResponseResult> UpdateAsync(Guid articleNumber, ProductEntity product)
    {
        try
        {
            var updateResult = await repo.UpdateAsync(x => x.ArticleNumber == articleNumber, product);

            if (updateResult.StatusCode == StatusCode.OK)
            {
                return ResponseFactory.Ok((ProductEntity)updateResult.ContentResult!);
            }
            else if (updateResult.StatusCode == StatusCode.NOT_FOUND)
                return ResponseFactory.NotFound();

            return ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"Error: ProductProvider.ProductService.UpdateAsync {ex.Message}");
            return ResponseFactory.Error();
        }
    }

    public async Task<ResponseResult> DeleteAsync(Guid articleNumber)
    {
        try
        {
            var result = await repo.DeleteAsync(x => x.ArticleNumber == articleNumber);

            if (result.StatusCode == StatusCode.OK)
            {
                return ResponseFactory.Ok();
            }
            else if (result.StatusCode == StatusCode.NOT_FOUND)
                return ResponseFactory.NotFound();

            return ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"Error: ProductProvider.ProductService.DeleteAsync {ex.Message}");
            return ResponseFactory.Error();
        }
    }
}
