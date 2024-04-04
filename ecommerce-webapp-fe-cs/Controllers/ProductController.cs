using Microsoft.AspNetCore.Mvc;
using ecommerce_webapp_fe_cs.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

    public async Task<IActionResult> Index()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7195/api/v1/products");
        var client = _clientFactory.CreateClient();
        var response = await client.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(jsonString);

            var productsArray = jObject["$values"]?.ToObject<List<Product>>();

            if (productsArray != null)
            {
                return View(productsArray);
            }
            else
            {
                _logger.LogError("Failed to extract products from JSON.");
                return View(new List<Product>());
            }
        }
        else
        {
            _logger.LogError("Failed to fetch products. Status code: {StatusCode}", response.StatusCode);
            return View(new List<Product>());
        }
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
    public class ProductResponse
    {
        [JsonProperty("$values")]
        public List<Product> Products { get; set; }
    }

}
