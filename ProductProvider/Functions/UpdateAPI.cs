using Infrastructure.Entities;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ProductProvider.Functions;

public class UpdateAPI(ILogger<UpdateAPI> logger, ProductService productService)
{
    private readonly ILogger<UpdateAPI> _logger = logger;
    private readonly ProductService _productService = productService;

    [Function("UpdateAPI")]
    public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "put")] HttpRequest req)
    {
        try
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();

            if (body != null)
            {
                var productModel = JsonConvert.DeserializeObject<UpdateProductModel>(body);

                if (productModel != null)
                {
                    var productEntity = new ProductEntity
                    {
                        ArticleNumber = productModel.ArticleNumber,
                        Price = productModel.Price,
                        Title = productModel.Title,
                        DiscountPrice = productModel.DiscountPrice,
                        Manufacturer = productModel.Manufacturer,
                        Ingress = productModel.Ingress,
                        Description = productModel.Description,
                        PrimaryImage = productModel.PrimaryImage
                    };
                    var product = await _productService.UpdateAsync(productEntity.ArticleNumber, productEntity);

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
