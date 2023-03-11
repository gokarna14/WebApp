using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    // Route attribute
    // decorated
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        [HttpGet]
        public IEnumerable<Product> GetProducts()
        {
            return new Product[]
            {
                new Product() {Name = "Product1"},
                new Product() {Name = "Product2"}
            };
        }

        [HttpGet("{id}")]
        public Product GetProduct()
        {
            return new Product {
                ProductId = 1, Name = "Test Product"
            };
        }
    }
}