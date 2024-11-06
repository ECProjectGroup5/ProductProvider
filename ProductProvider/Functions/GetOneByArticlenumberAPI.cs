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
    public class GetOneByArticlenumberAPI(ILogger<GetOneByArticlenumberAPI> logger, ProductService productService)
    {
        private readonly ILogger<GetOneByArticlenumberAPI> _logger = logger;
        private readonly ProductService _productService = productService;

        [HttpPost]
        [Function("GetOneByArticlenumberrAPI")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            try
            {
                var body = await new StreamReader(req.Body).ReadToEndAsync();

                if (!string.IsNullOrEmpty(body))
                {
                    var productRequest = JsonConvert.DeserializeObject<GetProductModel>(body);
                    var articlenumber = productRequest!.Articlenumber;


                    var result = await _productService.GetOneAsync(articlenumber);

                    return new OkObjectResult((ProductEntity)result.ContentResult!);
                }

                return new BadRequestResult();
            }

            catch (Exception ex)
            {
                _logger.LogDebug($"Error: ProductProvider.GetOneByArticleNumberAPI.RunAsync {ex.Message}");
                return new BadRequestResult(); ;
            }
        }
    }
}
