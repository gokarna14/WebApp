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
        public IEnumerable<Product> GetProducts()
        {
            return dataContext.Products;
        }

        [HttpGet("{id}")]
        public Product? GetProduct(long id, [FromServices] ILogger<ProductsController> logger)
        {
            logger.LogWarning("GetProduct Action Invoked");

            return dataContext.Products.Find(id);
        }

        [HttpPost]
        public void SaveProduct([FromBody] Product product){
            dataContext.Products.Add(product);
            dataContext.SaveChanges();
        }

        [HttpPut]
        public void UpdateProduct([FromBody] Product product)
        {
            dataContext.Products.Update(product);
            dataContext.SaveChanges();
        }

        [HttpDelete("{id}")]
        public void DeleteProduct(long id, [FromServices] ILogger<ProductsController> logger)
        {
            dataContext.Products.Remove(new Product {ProductId = id});
            dataContext.SaveChanges();
            logger.LogWarning($"The product with id {id} deleted !");
        }
    }
}