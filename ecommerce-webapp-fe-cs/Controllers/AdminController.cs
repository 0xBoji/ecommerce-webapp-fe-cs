using ecommerce_webapp_fe_cs.Models.AccountModels;
using ecommerce_webapp_fe_cs.Models.ProductModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ecommerce_webapp_fe_cs.Controllers;
public class AdminController(ILogger<ProductController> logger, IHttpClientFactory clientFactory) : Controller
{
	private readonly ILogger<ProductController> _logger = logger;
	private readonly IHttpClientFactory _clientFactory = clientFactory;

	public async Task<IActionResult> Index()
	{
		var userEmail = HttpContext.Session.GetString("UserEmail");
		if (string.IsNullOrEmpty(userEmail)) return RedirectToAction("Login");

		var client = _clientFactory.CreateClient();
		var response = await client.GetAsync($"https://localhost:7195/api/v1/accounts/profile?email={userEmail}");

		if (response.IsSuccessStatusCode)
		{
			return View();
		}
		else
		{
			return NotFound("This account is not an ADMIN.");
		}
	}

	public IActionResult Login() => View();

	[HttpPost]
	public async Task<IActionResult> Login(LoginAdminModel model)
	{
		if (ModelState.IsValid)
		{
			var client = _clientFactory.CreateClient();
			var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
			var response = await client.PostAsync("https://localhost:7195/api/v1/accounts/login-admin", content);

			if (response.IsSuccessStatusCode)
			{
				HttpContext.Session.SetString("UserEmail", model.Email);
				return RedirectToAction("Index");
			}
			else
			{
				ModelState.AddModelError(string.Empty, "Login failed.");
			}
		}
		return View(model);
	}

	public IActionResult Logout()
	{
		HttpContext.Session.Clear(); // Clears the session, effectively logging out the user
		return RedirectToAction("Login", "Account");
	}


	//public async Task<IActionResult> GetProductAsync()
	//{
	//	var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7195/api/v1/products");
	//	var client = _clientFactory.CreateClient();
	//	var response = await client.SendAsync(request);

	//	if (response.IsSuccessStatusCode)
	//	{
	//		var jsonString = await response.Content.ReadAsStringAsync();
	//		var jObject = JObject.Parse(jsonString);

	//		var productsArray = jObject["$values"]?.ToObject<List<Product>>();

	//		if (productsArray != null)
	//		{
	//			return View(productsArray);
	//		}
	//		else
	//		{
	//			_logger.LogError("Failed to extract products from JSON.");
	//			return View(new List<Product>());
	//		}
	//	}
	//	else
	//	{
	//		_logger.LogError("Failed to fetch products. Status code: {StatusCode}", response.StatusCode);
	//		return View(new List<Product>());
	//	}
	//}
	//public IActionResult PostProduct() => View();

	//[HttpPost]
	//public async Task<IActionResult> PostProduct(Product model, IFormFile file1, IFormFile file2, IFormFile file3)
	//{
	//	var client = _clientFactory.CreateClient();
	//	var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
	//	var response = await client.PostAsync("https://localhost:7195/api/v1/products", content);

	//	if (file1 != null && file1.Length > 0)
	//	{
	//		var filename1 = $"{DateTime.Now.Ticks}_{file1.FileName}";
	//		var filename2 = $"{DateTime.Now.Ticks}_{file2.FileName}";
	//		var filename3 = $"{DateTime.Now.Ticks}_{file3.FileName}";

	//		var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", filename1, filename2, filename3);
	//		using (var stream = new FileStream(filePath, FileMode.Create))
	//		{
	//			await file1.CopyToAsync(stream);
	//			await file2.CopyToAsync(stream);
	//			await file3.CopyToAsync(stream);
	//		}
	//		model.Image1 = filename1;
	//		model.Image2 = filename2;
	//		model.Image3 = filename3;
	//	}

	//	if (response.IsSuccessStatusCode)
	//	{
	//		return RedirectToAction("Index", "Home");
	//	}
	//	else
	//	{
	//		ModelState.AddModelError(string.Empty, "Add failed.");
	//	}
	//	return View(model);
	//}
}