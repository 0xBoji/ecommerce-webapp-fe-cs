using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ecommerce_webapp_fe_cs.Models.ProductModels;

namespace ecommerce_webapp_fe_cs.Controllers;
public class ProductController : Controller
{
    private readonly ILogger<ProductController> _logger;
    private readonly IHttpClientFactory _clientFactory;

    public ProductController(ILogger<ProductController> logger, IHttpClientFactory clientFactory)
    {
        _logger = logger;
        _clientFactory = clientFactory;
    }
    public IActionResult Index()
    {
        return View();
    }
    public async Task<IActionResult> Details(string id)
    {
        var requestUrl = $"https://localhost:7195/api/v1/products/{id}";
        var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
        var client = _clientFactory.CreateClient();

        try
        {
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<Product>(jsonString);

                if (product != null)
                {
                    return View(product);
                }
                else
                {
                    _logger.LogError("Failed to extract product details from JSON.");
                    return NotFound();
                }
            }
            else
            {
                _logger.LogError("Failed to fetch product details. Status code: {StatusCode}", response.StatusCode);
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching product details.");
            return StatusCode(500);
        }
    }
    [HttpGet("cart-list")]
    public IActionResult Cart()
    {
        return View();
    }
    public class ProductResponse
    {
        [JsonProperty("$values")]
        public List<Product> Products { get; set; }
    }

}
