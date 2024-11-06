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
    public class UpdateAPI(ILogger<UpdateAPI> logger, ProductService productService)
    {
        private readonly ILogger<UpdateAPI> _logger = logger;
        private readonly ProductService _productService = productService;

        [Function("UpdateAPI")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", "put")] HttpRequest req)
        {
            try
            {
                var body = await new StreamReader(req.Body).ReadToEndAsync();

                if (body != null)
                {
                    var productModel = JsonConvert.DeserializeObject<ProductEntity>(body);

                    if (productModel != null)
                    {
                        var product = await _productService.UpdateAsync(productModel.ArticleNumber, productModel);

                        if (product.StatusCode == StatusCode.OK)
                        {
                            return new OkObjectResult(product.ContentResult);
                        }
                        else if (product.StatusCode == StatusCode.NOT_FOUND)
                            return new NotFoundResult();
                    }
                }

                return new BadRequestResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in UpdateAPI.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
