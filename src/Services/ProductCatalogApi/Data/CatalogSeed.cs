using ProductCatalogApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogApi.Data
{
    public class CatalogSeed
    {
        public static async Task SeedAsync(CatalogContext context)
        {
            if (!context.catalogBrands.Any())
            {
                context.catalogBrands.AddRange(GetPreConfiguredCatalogBrands());
                await context.SaveChangesAsync();
            }
            if (!context.CatalogTypes.Any())
            {
                context.CatalogTypes.AddRange(GetPreConfiguredCatalogTypes());
                await context.SaveChangesAsync();
            }
            if (!context.CatalogItems.Any())
            {
                context.CatalogItems.AddRange(GetPreConfiguredCatalogItems());
                await context.SaveChangesAsync();
            }
        }

        static IEnumerable<CatalogBrand> GetPreConfiguredCatalogBrands()
        {
            return new List<CatalogBrand>
            {
                new CatalogBrand(){ Brand = "Addidas" },
                new CatalogBrand(){ Brand = "Nikes" },
                new CatalogBrand(){ Brand = "Slazenger" }
            };
        }

        static IEnumerable<CatalogType> GetPreConfiguredCatalogTypes()
        {
            return new List<CatalogType>
            {
                new CatalogType(){ Type = "Basketball" },
                new CatalogType(){ Type = "Football" },
                new CatalogType(){ Type = "Tennis" }
            };
        }

        static IEnumerable<CatalogItem> GetPreConfiguredCatalogItems()
        {
            return new List<CatalogItem>{
                new CatalogItem() { CatalogBrandId = 1, CatalogTypeId = 2, Description = "Shoes for next centry", Name = "World Start", Price = 199.5M, PictureUrl = "http://externalcatalogbaseurltobereplaces/api/pic/1" },
                new CatalogItem() { CatalogBrandId = 1, CatalogTypeId = 2, Description = "Shoes for next centry", Name = "World Start", Price = 199.5M, PictureUrl = "http://externalcatalogbaseurltobereplaces/api/pic/1" }
            };
        }
    }
}
