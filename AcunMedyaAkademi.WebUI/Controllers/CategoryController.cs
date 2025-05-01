using AcunMedyaAkademi.WebUI.DTOs.CategoryDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
    }
}
