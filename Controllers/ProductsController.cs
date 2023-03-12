using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    [ApiController]
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
        public async Task<IActionResult> GetProduct(long id, [FromServices] ILogger<ProductsController> logger)
        {
            logger.LogWarning("GetProduct Action Invoked");

            Product? p = await dataContext.Products.FindAsync(id);

            if (p == null)
            {
                return NotFound();
            }

            return Ok(p);
        }

        [HttpPost]
        // API Controller means that you dont need to specify [FromBody]
        public async Task<IActionResult> SaveProduct(ProductBindingTarget product)
        {
                Product p = product.ToProduct();
                await dataContext.Products.AddAsync(p);
                await dataContext.SaveChangesAsync();

                //returns p as response as well
                return Ok(p);
        }

        [HttpPut]
        // API Controller means that you dont need to specify [FromBody]
        public async Task UpdateProduct(Product product)
        {
            dataContext.Products.Update(product);
            await dataContext.SaveChangesAsync();
        }

        [HttpDelete("{id}")]
        public async Task DeleteProduct(long id)
        {
            dataContext.Products.Remove(new Product { ProductId = id });
            await dataContext.SaveChangesAsync();
            // logger.LogWarning($"The product with id {id} deleted !");
        }

        [HttpGet("redirect")]
        public IActionResult Redirect()
        {
            // return Redirect("/api/products/1");
            return RedirectToAction(nameof(GetProduct), new { Id = 1 });
        }

        [HttpGet("redirect/{id}")]
        public IActionResult Redirect(long id)
        {
            // return Redirect("/api/products/1");
            return RedirectToAction(nameof(GetProduct), new { Id = id });
        }
        [HttpGet("redirectWithRoute")]
        public IActionResult RedirectWithRoute()
        {
            return RedirectToRoute(new
            {
                controller = "Products",
                action = "GetProduct",
                Id = 1
            });
        }
    }
}