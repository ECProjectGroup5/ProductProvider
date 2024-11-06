using Infrastructure.Entities;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ProductProvider.Functions;

public class GetAllAPI(ILogger<GetAllAPI> logger, ProductService productService)
{
    private readonly ILogger<GetAllAPI> _logger = logger;
    private readonly ProductService _productService = productService;

    [Function("GetAllAPI")]
    public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
    {
        try
        {
            var result = await _productService.GetAllAsync();

            if (result.StatusCode == StatusCode.OK)
            {
                return new OkObjectResult((IEnumerable<ProductEntity>)result.ContentResult!);
            }
            else if (result.StatusCode == StatusCode.NOT_FOUND)
            {
                return new NotFoundResult();
            }

            return new BadRequestResult();
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"Error: ProductProvider.GetAllAPI.Run {ex.Message}");
            return new BadRequestResult();
        }
    }
}
