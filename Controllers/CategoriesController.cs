using My_app_backend.Models;
using My_app_backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace My_app_backend.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        private readonly ArticleService _articleService;

        public CategoriesController(CategoryService categoryService, ArticleService articleService)
        {
            _categoryService = categoryService;
            _articleService = articleService;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<List<Category>> Get() =>
            _categoryService.Get();

        [HttpGet("{id:length(24)}", Name = "GetCategory")]
        [Authorize]
        public ActionResult<Category> Get(string id)
        {
            var category = _categoryService.Get(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<Category> Create(Category category)
        {
            if(_categoryService.ExistCategoryName(category.Name))
            {
                return BadRequest($"Já existe uma Categoria com o nome de {category.Name}");
            }
            _categoryService.Create(category);

            return CreatedAtRoute("GetCategory", new { id = category.Id.ToString() }, category);
        }

        [HttpPut("{id:length(24)}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(string id, Category categoryIn)
        {
            var category = _categoryService.Get(id);

            if (category == null)
            {
                return NotFound();
            }

            _categoryService.Update(id, categoryIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string id)
        {
            var category = _categoryService.Get(id);
            if (category == null)
            {
                return NotFound();
            }
            var article = _articleService.GetArticlesByCategory(id);
            if(article != null)
            {
                return BadRequest("Categoria possui Artigos relacionados a ela");
            } 
            
            if(_categoryService.ExistSubCategories(category.Name))
            {
                return BadRequest("A Categoria possui sub Categorias");
            }
            _categoryService.Remove(category.Id);
            return NoContent();
        }
    }
}