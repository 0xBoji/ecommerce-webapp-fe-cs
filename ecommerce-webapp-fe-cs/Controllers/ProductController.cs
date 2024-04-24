using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ecommerce_webapp_fe_cs.Models.ProductModels;
using System.Text;

namespace ecommerce_webapp_fe_cs.Controllers
{
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(string proId, int quantity)
        {
            string userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError("User ID is not available in session.");
                return Json(new { success = false, message = "User is not logged in." });
            }

            var requestUrl = "https://localhost:7195/api/v1/productOrder/cart";
            var postData = new
            {
                userId = userId,
                proId = proId,
                quantity = quantity
            };

            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
            {
                Content = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json")
            };

            try
            {
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "Product added to cart successfully!" });
                }
                else
                {
                    _logger.LogError("Failed to add product to cart. Status code: {StatusCode}", response.StatusCode);
                    return Json(new { success = false, message = "Error adding product to cart." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding product to cart.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
