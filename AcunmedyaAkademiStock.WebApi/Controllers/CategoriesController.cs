using AcunmedyaAkademiStock.WebApi.Context;
using AcunmedyaAkademiStock.WebApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcunmedyaAkademiStock.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApiContext _context;

        public CategoriesController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult CategoryList()
        {
            var value = _context.Categories.ToList();
           
            if(value != null) {

            return Ok(value);
               
            }
            return BadRequest("Veri bulunamıyor veya erişilemiyor");
            
        }

        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            if(category != null) {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return Ok("Ekeleme Başarılı");
            }
            return BadRequest("Ekleme işlemi başarısız");
        }
        
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id) {
            
            var value = _context.Categories.Find(id);
                
            if(value != null) {
                _context.Categories.Remove(value);
                _context.SaveChanges();
                return Ok("Silme işlemi başarılı");
                }
            return BadRequest("Silme işlemi başarısız");
         }

        [HttpPut]
        public IActionResult UpdateCategory(Category category) {
            if(category != null) {
                _context.Categories.Update(category);
                _context.SaveChanges();
                return Ok("Güncelleme başarılı");
                }
            return BadRequest("Güncelleme başarısız");

        }
    }
}
