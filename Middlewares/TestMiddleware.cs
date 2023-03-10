using WebApp.Data;
using WebApp.Models;

namespace WebApp.Middlewares
{
    public class TestMiddleware
    {
        RequestDelegate requestDelegate;

        public TestMiddleware(RequestDelegate requestDelegate)
        {
            this.requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext context, DataContext dataContext)
        {
            if (context.Request.Path == "/test")
            {
                await context.Response.WriteAsync($"There are {dataContext.Products.Count()} products\n");
                await context.Response.WriteAsync($"There are {dataContext.Categories.Count()} products\n");
                await context.Response.WriteAsync($"There are {dataContext.Suppliers.Count()} products\n");
            }
            else
            {
                await requestDelegate(context);
            }
        }
    }
}