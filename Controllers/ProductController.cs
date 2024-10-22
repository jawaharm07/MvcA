using Microsoft.AspNetCore.Mvc;
using MvcAPI.Models;
using Newtonsoft.Json;

namespace MvcAPI.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7247/Api");
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<ProductViewModel> productviewmodel = new List<ProductViewModel>();

            try
            {
                // Since BaseAddress is set, use relative path
                HttpResponseMessage response = await _httpClient.GetAsync("Product/Get");

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    productviewmodel = JsonConvert.DeserializeObject<List<ProductViewModel>>(data);
                }
                else
                {
                    // Handle unsuccessful response
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (if logging is configured)
                ModelState.AddModelError(string.Empty, "An error occurred while fetching the data.");
            }

            return View(productviewmodel);
        }
    }

}
