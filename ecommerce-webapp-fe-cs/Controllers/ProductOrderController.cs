using ecommerce_webapp_fe_cs.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using ecommerce_webapp_fe_cs.Models.ProductModels;
using System.Diagnostics;

namespace ecommerce_webapp_fe_cs.Controllers;
public class ProductOrderController : Controller
{
    private readonly ILogger<ProductController> _logger;
    private readonly IHttpClientFactory _clientFactory;
    public ProductOrderController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }
    [HttpGet]
    public async Task<IActionResult> Cart(int id)
    {
        return View();
    }
}
