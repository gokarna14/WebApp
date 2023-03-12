using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    // Route attribute
    // decorated
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private DataContext dataContext;

        public ProductsController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [HttpGet]
        public IAsyncEnumerable<Product> GetProducts()
        {
            return dataContext.Products.AsAsyncEnumerable();
        }

        [HttpGet("{id}")]
        public async Task<Product?> GetProduct(long id, [FromServices] ILogger<ProductsController> logger)
        {
            logger.LogWarning("GetProduct Action Invoked");

            return await dataContext.Products.FindAsync(id);
        }

        [HttpPost]
        public async Task SaveProduct([FromBody] ProductBindingTarget product){
            await dataContext.Products.AddAsync(product.ToProduct());
            await dataContext.SaveChangesAsync();
        }

        [HttpPut]
        public async Task UpdateProduct([FromBody] Product product)
        {
            dataContext.Products.Update(product);
            await dataContext.SaveChangesAsync();
        }

        [HttpDelete("{id}")]
        public async Task DeleteProduct(long id)
        {
            dataContext.Products.Remove(new Product {ProductId = id});
            await dataContext.SaveChangesAsync();
            // logger.LogWarning($"The product with id {id} deleted !");
        }
    }
}