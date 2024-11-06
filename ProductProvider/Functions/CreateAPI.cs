using Infrastructure.Entities;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ProductProvider.Functions
{
    public class CreateAPI(ILogger<CreateAPI> logger, ProductService productService)
    {
        private readonly ILogger<CreateAPI> _logger = logger;
        private readonly ProductService _productService = productService;

        [HttpPost]
        [Function("CreateAPI")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            try
            {
                var body = await new StreamReader(req.Body).ReadToEndAsync();
                if (!string.IsNullOrWhiteSpace(body))
                {
                    var productModel = JsonConvert.DeserializeObject<ProductModel>(body);

                    if (productModel == null)
                    {
                        return new BadRequestResult();
                    }

                    var result = await _productService.CreateAsync(productModel);

                    if (result.StatusCode == StatusCode.OK)
                    {
                        return new OkObjectResult((ProductEntity)result.ContentResult!);
                    }
                }

                return new BadRequestResult();
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"Error: ProductProvider.CreateAPI.RunAsync {ex.Message}");
                return new BadRequestResult();
            }
        }
    }
}
