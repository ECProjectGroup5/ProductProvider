using Infrastructure.Entities;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ProductProvider.Functions;

public class GetOneAPI(ILogger<GetOneAPI> logger, ProductService productService)
{
    private readonly ILogger<GetOneAPI> _logger = logger;
    private readonly ProductService _productService = productService;


    [HttpGet]
    [Function("GetOneAPI")]
    public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
    {
        try
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();

            if (body != null)
            {
                var product = JsonConvert.DeserializeObject<GetProductModel>(body);
                var articlenumber = product!.Articlenumber;


                var result = await _productService.GetOneAsync(articlenumber);

                return new OkObjectResult((ProductEntity)result.ContentResult!);
            }

            return new BadRequestResult();
        }

        catch (Exception ex)
        {
            _logger.LogDebug($"Error: ProductProvider.GetOneAPI.RunAsync {ex.Message}");
            return new BadRequestResult(); ;
        }
    }
}
