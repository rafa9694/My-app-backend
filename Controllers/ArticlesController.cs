using My_app_backend.Models;
using My_app_backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace My_app_backend.Controllers
{
    [Route("api/article")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly ArticleService _articleService;
        private readonly CategoryService _categoryService;

        public ArticlesController(ArticleService articleService, CategoryService categoryService)
        {
            _articleService = articleService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public ActionResult<List<Article>> Get() 
        {
            var list = _articleService.Get();
             list.ForEach(element => 
             element.Category_Id = _categoryService.Get(element.Category_Id).Name
            );
            return list;
        }

        [HttpGet("{id:length(24)}", Name = "GetArticle")]
        public ActionResult<Article> Get(string id)
        {
            var article = _articleService.Get(id);
            if (article == null)
            {
                return NotFound();
            }
            
            article.Category_Id = _categoryService.Get(article.Category_Id).Name;
            
            return article;
        }

        [HttpPost]
        public ActionResult<Article> Create(Article article)
        {
             var result =_articleService.Create(article);
            if(result != "Sucesso") {
                return NotFound(result);
            }
            return CreatedAtRoute("GetArticle", new { id = article.Id.ToString() }, article);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Article articleIn)
        {
            var article = _articleService.Get(id);
            
            if (article == null)
            {
                return NotFound();
            }

            _articleService.Update(id, articleIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var article = _articleService.Get(id);

            if (article == null)
            {
                return NotFound();
            }

            _articleService.Remove(article.Id);

            return NoContent();
        }
    }
}