

using Infrastructure.Entities;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

namespace Infrastructure.Factories;

public class ProductFactory
{

    public ProductEntity PopulateProduct(string body)
    {
        return JsonConvert.DeserializeObject<ProductEntity>(body)!;
       
    }

    public ProductEntity PopulateProduct(ProductEntity productEntity)
    {
        return new ProductEntity
        {


            Price = productEntity.Price,
            DiscountPrice = productEntity.DiscountPrice,
            Title = productEntity.Title,
            Description = productEntity.Description,
            Ingress = productEntity.Ingress,
            Manufacturer = productEntity.Manufacturer,
            PrimaryImage = productEntity.PrimaryImage,

        };

    }


}
