/*using ecommerce_webapp_fe_cs.Models;
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
		ViewBag.Id = id;
		return View();
	}
    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int amount)
    {
        var client = _clientFactory.CreateClient();
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("ProductId", productId.ToString()),
            new KeyValuePair<string, string>("Amount", amount.ToString())
        });

        var response = await client.PostAsync("https://localhost:7195/api/v1/productOrder/cart", content);

        if (response.IsSuccessStatusCode)
        {
            // redirect to the cart view or display a success message
            return RedirectToAction("ViewCart");
        }
        else
        {
            // handle errors, e.g., show an error message or log the issue
            return RedirectToAction("Details", new { id = productId });
        }
    }
    public async Task<IActionResult> OrderList()
    {
        var UserId = HttpContext.Session.Id;
        var requestUrl = $"https://localhost:7195/api/v1/productOrder/{UserId}";
        var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
        var client = _clientFactory.CreateClient();

        try
        {
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<CardModel>(jsonString);

                if (order != null)
                {

                    return View(order);
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

[HttpGet("cart")]
public async Task<IActionResult> ViewCart()
{
    var userIdString = HttpContext.Session.GetString("UserID");
    if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
    {
        // Redirect user to login page
        return RedirectToAction("Login", "Account"); // Update to your login page if different
    }

    var client = _clientFactory.CreateClient();
    var response = await client.GetAsync($"https://localhost:7195/api/v1/productOrder/cart/{userId}");
        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var cartData = JsonConvert.DeserializeObject<YourCartModel>(jsonString); // Replace 'YourCartModel' with your actual cart model

            return View(cartData); // Pass the cart data to the Razor view
        }
        else
        {
            // Handle API error response, e.g., by displaying an error message or redirecting to an error page
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
    public class CartOrder
    {
        public string OrderId { get; set; }
        public int UserId { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        public class OrderItem
        {
            public string OrderItemId { get; set; }
            public string ProId { get; set; }
            public string ProName { get; set; }
            public string Description { get; set; }
            public string ProImg1 { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
        }
    }
}
*/