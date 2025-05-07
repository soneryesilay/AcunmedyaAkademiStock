using AcunMedyaAkademi.WebUI.DTOs.CategoryDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AcunMedyaAkademi.WebUI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CategoryController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> CategoryList()
        {
            //http client factoryi kullanarak yeni istek oluştur
            var client = _httpClientFactory.CreateClient();
            //response messagın nereden geldiğini tanımla
            var responseMessage = await client.GetAsync("https://localhost:7053/api/Categories"); 
            //eğer mesaj 200 ile dönerse parantezdeki işlemleri yap!
            if(responseMessage.IsSuccessStatusCode)
            {
                //json datasını json dataya taşıyoruz
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                //metne çevirme
                var values = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(jsonData);
                return View(values);
            }
            return View();
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto dto)
        {
            if(dto!=null) {
                var client = _httpClientFactory.CreateClient();
                var jsonData = JsonConvert.SerializeObject(dto);
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                await client.PostAsync("https://localhost:7053/api/Categories", content);
                return RedirectToAction("CategoryList");
            }
            return View("CategoryList");
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"https://localhost:7053/api/Categories/{id}");
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("CategoryList");
            }
            var errorMessage = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"Silme işlemi başarısız: {errorMessage}");
            return RedirectToAction("CategoryList");
        }
    }
}
