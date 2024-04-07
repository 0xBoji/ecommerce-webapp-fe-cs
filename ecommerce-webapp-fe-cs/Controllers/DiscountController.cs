using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ecommerce_webapp_fe_cs.Models.ProductModels;

namespace ecommerce_webapp_fe_cs.Controllers;
public class DiscountController(ILogger<DiscountController> logger, IHttpClientFactory clientFactory) : Controller
{
    private readonly ILogger<DiscountController> _logger = logger;
    private readonly IHttpClientFactory _clientFactory = clientFactory;

    public async Task<IActionResult> Index()
    {
        // ***
        var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7195/api/v1/discounts"); 
        // ***
        var client = _clientFactory.CreateClient();
        var response = await client.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(jsonString);

            var productsArray = jObject["$values"]?.ToObject<List<Discount>>();

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

    public class DiscountResponse
    {
        [JsonProperty("$values")]
        public List<Discount> Discounts  { get; set; }
    }
}
