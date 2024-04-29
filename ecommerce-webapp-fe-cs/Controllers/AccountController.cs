using Microsoft.AspNetCore.Mvc;
using System.Text;
using Newtonsoft.Json;
using ecommerce_webapp_fe_cs.Models;
using Microsoft.AspNetCore.Http;
using ecommerce_webapp_fe_cs.Models.AccountModels;

namespace ecommerce_webapp_fe_cs.Controllers;

public class AccountController(IHttpClientFactory clientFactory) : Controller
{
	private readonly IHttpClientFactory _clientFactory = clientFactory;

	public IActionResult SignUp()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> SignUp(UserRegistrationModel model)
	{
		if (ModelState.IsValid)
		{
			var client = _clientFactory.CreateClient();
			var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
			var response = await client.PostAsync("https://localhost:7195/api/v1/accounts/signup", content);

			if (response.IsSuccessStatusCode)
			{
				return RedirectToAction("Index", "Home");
			}
			else
			{
				ModelState.AddModelError(string.Empty, "Registration failed.");
			}
		}
		return View(model);
	}
	public IActionResult GoogleLogin()
	{
		return Redirect("https://localhost:7195/google/login");
	}

	public IActionResult GoogleLoginCallback(string token)
	{
		var cookieOptions = new CookieOptions
		{
			HttpOnly = true,
			Secure = true, // Set to true if using HTTPS, which is recommended
			Expires = DateTime.UtcNow.AddDays(7) // Set the cookie to expire when the token does
		};
		Response.Cookies.Append("JWTToken", token, cookieOptions);

		return RedirectToAction("Profile");
	}


	public IActionResult Login()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> Login(LoginModel model)
	{
		if (ModelState.IsValid)
		{
			var client = _clientFactory.CreateClient();
			var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
			var response = await client.PostAsync("https://localhost:7195/api/v1/accounts/login", content);

			if (response.IsSuccessStatusCode)
			{
				HttpContext.Session.SetString("UserEmail", model.Email);
				return RedirectToAction("Profile");
			}
			else
			{
				ModelState.AddModelError(string.Empty, "Login failed.");
			}
		}
		return View(model);
	}


	public async Task<IActionResult> Profile()
	{
		ProfileModel profileModel = null;

		var userEmail = HttpContext.Session.GetString("UserEmail");

		var client = _clientFactory.CreateClient();

		if (!string.IsNullOrEmpty(userEmail))
		{
			var response = await client.GetAsync($"https://localhost:7195/api/v1/accounts/profile?email={userEmail}");
			if (response.IsSuccessStatusCode)
			{
				var jsonString = await response.Content.ReadAsStringAsync();
				profileModel = JsonConvert.DeserializeObject<ProfileModel>(jsonString);
			}
		}
		else
		{
			// If no email, try to get JWT token (Google login)
			var token = Request.Cookies["JWTToken"];
			if (!string.IsNullOrEmpty(token))
			{
				client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
				var response = await client.GetAsync("https://localhost:7195/api/v1/accounts/profile");

				if (response.IsSuccessStatusCode)
				{
					var jsonString = await response.Content.ReadAsStringAsync();
					profileModel = JsonConvert.DeserializeObject<ProfileModel>(jsonString);
				}
			}
		}

		if (profileModel != null)
		{
			return View(profileModel); // Return the profile view with the model
		}
		else
		{
			return Unauthorized("User is not authenticated."); // Or redirect to login
		}
	}


	public IActionResult CaptureToken(string token)
	{
		if (!string.IsNullOrEmpty(token))
		{
			Response.Cookies.Append("JWTToken", token, new CookieOptions { HttpOnly = true, Secure = true });
			return RedirectToAction("Profile");
		}

		return Unauthorized("Token is missing or invalid.");
	}


	[HttpPost]
	public IActionResult Logout()
	{
		HttpContext.Session.Clear(); // Clears the session
		Response.Cookies.Delete("JWTToken"); // Clears the JWT token cookie
		return RedirectToAction("Login"); // Redirects to the login page
	}


	public async Task<IActionResult> ProfileEdit()
	{
		var userEmail = HttpContext.Session.GetString("UserEmail");
		if (string.IsNullOrEmpty(userEmail))
		{
			return Unauthorized("User is not authenticated.");
		}

		var client = _clientFactory.CreateClient();
		var response = await client.GetAsync($"https://localhost:7195/api/v1/accounts/profile?email={userEmail}");

		if (response.IsSuccessStatusCode)
		{
			var jsonString = await response.Content.ReadAsStringAsync();
			var profileModel = JsonConvert.DeserializeObject<ProfileEditModel>(jsonString);
			return View(profileModel);
		}
		else
		{
			return NotFound("Profile not found.");
		}
	}

	[HttpPost]
	public async Task<IActionResult> ProfileEdit(ProfileEditModel model, IFormFile file)
	{
		//check if login or not
		var userEmail = HttpContext.Session.GetString("UserEmail");
		if (string.IsNullOrEmpty(userEmail))
		{
			return Unauthorized("User is not authenticated.");
		}

		if (file != null && file.Length > 0)
		{
			var filename = $"{DateTime.Now.Ticks}_{file.FileName}";
			var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", filename);
			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}
			model.UserImg = filename;
		}

		var client = _clientFactory.CreateClient();
		var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
		var response = await client.PutAsync($"https://localhost:7195/api/v1/accounts/profile/edit?email={userEmail}", content);

		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("Profile", "Account");
		}
		else
		{
			ModelState.AddModelError(string.Empty, "Edit failed.");
		}
		return View(model);
	}
}