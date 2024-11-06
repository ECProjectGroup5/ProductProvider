using Infrastructure.Entities;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ProductProvider;

public class DeleteAPI(ILogger<DeleteAPI> logger, ProductService productService)
{
    private readonly ILogger<DeleteAPI> _logger = logger;
    private readonly ProductService _productService = productService;

    [HttpDelete]
    [Function("DeleteAPI")]
    public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "delete")] HttpRequest req)
    {
        try
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();

            if (!string.IsNullOrWhiteSpace(body))
            {
                var product = JsonConvert.DeserializeObject<GetProductModel>(body);
                var articlenumber = product?.Articlenumber;

                if (articlenumber!.GetType() == typeof(Guid))
                {
                    var result = await _productService.DeleteAsync((Guid)articlenumber);

                    if (result.StatusCode == StatusCode.OK)
                    {
                        return new OkResult();
                    }
                    else if (result.StatusCode == StatusCode.NOT_FOUND)
                    {
                        return new NotFoundResult();
                    }
                }
            }

            return new BadRequestResult();
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"Error: ProductProvider.DeleteAPI.RunAsync {ex.Message}");
            return new BadRequestResult();
        }
    }
}