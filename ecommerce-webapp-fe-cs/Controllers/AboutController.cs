using Microsoft.AspNetCore.Mvc;

namespace ecommerce_webapp_fe_cs.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AboutController(IHttpClientFactory clientFactory) : Controller
{
    private readonly IHttpClientFactory _clientFactory = clientFactory;

    public IActionResult Index() => View();

}
