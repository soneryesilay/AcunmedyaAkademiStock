using AcunMedyaAkademi.WebUI.DTOs.CategoryDtos;
using AcunMedyaAkademi.WebUI.DTOs.ProductDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AcunMedyaAkademi.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> ProductList()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7053/api/Products");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultProductDto>>(jsonData);
                return View(values);
            }
            return View();
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto createProductDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(createProductDto);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            await client.PostAsync("https://localhost:7053/api/Products", content);
            return RedirectToAction("ProductList");
        }
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"https://localhost:7053/api/Products/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ProductList");
            }
            var errorMessage = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"Silme işlemi başarısız: {errorMessage}");
            return RedirectToAction("ProductList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7053/api/Products/GetProduct?id={id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<UpdateProductDto>(jsonData);
                return View(product);
            }

            // Handle errors (e.g., product not found)
            return RedirectToAction("ProductList");
        }



        [HttpPost]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProductDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(updateProductDto);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // API endpoint'ini kontrol edin - "/" son karakteri olmamalı
            var response = await client.PutAsync("https://localhost:7053/api/Products", content);

            // İsteğin başarılı olup olmadığını kontrol edin
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ProductList");
            }
            else
            {
                // Hata mesajını loglamak veya göstermek için
                var errorContent = await response.Content.ReadAsStringAsync();
                // Hata bilgisini View'e iletebilir veya loglayabilirsiniz
                ModelState.AddModelError("", "Güncelleme işlemi başarısız: " + errorContent);
                return View(updateProductDto);
            }
        }
    }
}
