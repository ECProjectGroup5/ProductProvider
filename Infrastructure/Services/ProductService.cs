using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

// Den här klassen hanterar tjänstelogiken för att arbeta med produkter
public class ProductService(IRepo repo, ILogger<ProductService> logger , ProductFactory productFactory)
{
    // Fält för att hålla referenser till repo och logger
    private readonly IRepo repo = repo;
    private readonly ILogger<ProductService> _logger = logger;
    private readonly ProductFactory _productFactory = productFactory;

    // Skapar en ny produkt baserat på den data som skickas in
    public async Task<ResponseResult> CreateAsync(string body)
    {
        try
        {
            var productEntity = _productFactory.PopulateProduct(body);
            productEntity = _productFactory.PopulateProduct(productEntity);

            if (productEntity.GetType() == typeof(ProductEntity))
            {
                var result = await repo.CreateAsync(productEntity);

                // Kollar om produkten skapades korrekt och returnerar resultatet
                if (result.StatusCode == StatusCode.OK)
                {
                    return ResponseFactory.Ok((ProductEntity)result.ContentResult!);
                }
            }
            // Returnerar ett fel om något gick fel vid skapandet
            return ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            // Loggar ett felmeddelande om ett undantag inträffar
            _logger.LogDebug($"Error: ProductProvider.ProductService.CreateAsync {ex.Message}");
            return ResponseFactory.Error();
        }
    }

    // Hämtar en specifik produkt baserat på artikelnummer
    public async Task<ResponseResult> GetOneAsync(Guid articleNumber)
    {
        try
        {
            // Använder repo för att leta efter produkten i databasen
            var result = await repo.GetOneAsync(x => x.ArticleNumber == articleNumber);

            // Om produkten hittas returneras den
            if (result.StatusCode == StatusCode.OK)
            {
                return ResponseFactory.Ok((ProductEntity)result.ContentResult!);
            }

            // Returnerar NotFound om produkten inte hittas
            return ResponseFactory.NotFound();
        }
        catch (Exception ex)
        {
            // Loggar ett felmeddelande om ett undantag inträffar
            _logger.LogDebug($"Error: ProductProvider.ProductService.GetOneAsync {ex.Message}");
            return ResponseFactory.Error();
        }
    }

    // Hämtar alla produkter från databasen
    public async Task<ResponseResult> GetAllAsync()
    {
        try
        {
            // Använder repo för att hämta alla produkter
            var result = await repo.GetAllAsync();

            // Returnerar produkterna om de hittas
            if (result.StatusCode == StatusCode.OK)
            {
                return ResponseFactory.Ok((IEnumerable<ProductEntity>)result.ContentResult!);
            }
            else if (result.StatusCode == StatusCode.NOT_FOUND)
                return ResponseFactory.NotFound();

            // Returnerar ett fel om något annat gick fel
            return ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            // Loggar ett felmeddelande om ett undantag inträffar
            _logger.LogDebug($"Error: ProductProvider.ProductService.GetAllAsync {ex.Message}");
            return ResponseFactory.Error();
        }
    }

    // Uppdaterar en befintlig produkt baserat på artikelnummer
    public async Task<ResponseResult> UpdateAsync(Guid articleNumber, ProductEntity product)
    {
        try
        {
            // Använder repo för att uppdatera produkten
            var updateResult = await repo.UpdateAsync(x => x.ArticleNumber == articleNumber, product);

            // Returnerar den uppdaterade produkten om uppdateringen lyckades
            if (updateResult.StatusCode == StatusCode.OK)
            {
                return ResponseFactory.Ok((ProductEntity)updateResult.ContentResult!);
            }
            else if (updateResult.StatusCode == StatusCode.NOT_FOUND)
                return ResponseFactory.NotFound();

            // Returnerar ett fel om något annat gick fel
            return ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            // Loggar ett felmeddelande om ett undantag inträffar
            _logger.LogDebug($"Error: ProductProvider.ProductService.UpdateAsync {ex.Message}");
            return ResponseFactory.Error();
        }
    }

    // Tar bort en produkt baserat på artikelnummer
    public async Task<ResponseResult> DeleteAsync(Guid articleNumber)
    {
        try
        {
            // Använder repo för att ta bort produkten
            var result = await repo.DeleteAsync(x => x.ArticleNumber == articleNumber);

            // Returnerar OK om produkten togs bort korrekt
            if (result.StatusCode == StatusCode.OK)
            {
                return ResponseFactory.Ok();
            }
            else if (result.StatusCode == StatusCode.NOT_FOUND)
                return ResponseFactory.NotFound();

            // Returnerar ett fel om något annat gick fel
            return ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            // Loggar ett felmeddelande om ett undantag inträffar
            _logger.LogDebug($"Error: ProductProvider.ProductService.DeleteAsync {ex.Message}");
            return ResponseFactory.Error();
        }
    }
}
