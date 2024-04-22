using ecommerce_webapp_fe_cs.Models.AccountModels;
using ecommerce_webapp_fe_cs.Models.ProductModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ecommerce_webapp_fe_cs.Controllers;
[Route("[controller]")]
public class AdminController(ILogger<ProductController> logger, IHttpClientFactory clientFactory) : Controller
{
	private readonly ILogger<ProductController> _logger = logger;
	private readonly IHttpClientFactory _clientFactory = clientFactory;

	public IActionResult Index() => View();

	[HttpGet("login-admin")]
	public IActionResult LoginAdmin() => View();

	[HttpPost("login-admin")]
	public async Task<IActionResult> LoginAdmin(LoginModel model)
	{
		if (ModelState.IsValid)
		{
			var client = _clientFactory.CreateClient();
			var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
			var response = await client.PostAsync("https://localhost:7195/api/v1/accounts/login-admin", content);

			if (response.IsSuccessStatusCode)
			{
				return RedirectToAction("Index", "Admin");
			}
			else
			{
				ModelState.AddModelError(string.Empty, "Login failed. Please check your credentials and try again.");
			}
		}
		return View(model);
	}

	[HttpPost]
	public IActionResult Logout()
	{
		HttpContext.Session.Clear();
		Response.Cookies.Delete("JWTToken");
		return RedirectToAction("Login");
	}

	[HttpGet("product-list")]
	public IActionResult ShowList() => View();


	[HttpGet("blog-list")]
	public IActionResult ShowBlogs() => View();


	[HttpGet("nego-list")]
	public IActionResult ShowNegotiations() => View();

	public async Task<IActionResult> GetProductAsync()
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

	public IActionResult PostProduct() => View();

	[HttpPost]
	public async Task<IActionResult> PostProduct(Product model, IFormFile file1, IFormFile file2, IFormFile file3)
	{
		var client = _clientFactory.CreateClient();
		var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
		var response = await client.PostAsync("https://localhost:7195/api/v1/products", content);

		if (file1 != null && file1.Length > 0)
		{
			var filename1 = $"{DateTime.Now.Ticks}_{file1.FileName}";
			var filename2 = $"{DateTime.Now.Ticks}_{file2.FileName}";
			var filename3 = $"{DateTime.Now.Ticks}_{file3.FileName}";

			var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", filename1, filename2, filename3);
			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file1.CopyToAsync(stream);
				await file2.CopyToAsync(stream);
				await file3.CopyToAsync(stream);
			}
			model.Image1 = filename1;
			model.Image2 = filename2;
			model.Image3 = filename3;
		}

		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("Index", "Home");
		}
		else
		{
			ModelState.AddModelError(string.Empty, "Add failed.");
		}
		return View(model);

	}
}