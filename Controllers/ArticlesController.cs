using My_app_backend.Models;
using My_app_backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace My_app_backend.Controllers
{
    [Route("api/article")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly ArticleService _articleService;
        private readonly CategoryService _categoryService;
        private readonly UserService _userService;

        public ArticlesController(
            ArticleService articleService, 
            CategoryService categoryService,
            UserService userService)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<List<Article>> Get() 
        {
            var list = _articleService.Get();

             list.ForEach(element => 
             element.Category_Id = _categoryService.Get(element.Category_Id).Name
            );

            return list;
        }

        [HttpGet("category/{id:length(24)}/{page}")]
        [Authorize]
        public ActionResult<List<Article>> GetByCategoryName(string id, int page) 
        {
            var category= _categoryService.Get(id);
            if(category == null) 
            {
                BadRequest(new { message = "O artigo não possui categoria associada a ele!" });
            }
            var articles = _articleService.PaginationArticlesByCategory(category.Id, page);
            return articles;
        }

        [HttpGet("{id:length(24)}", Name = "GetArticle")]
        [Authorize]
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
        [Authorize]
        public ActionResult<Article> Create(Article article)
        {
            var user = _userService.GetDtoByName(article.user.Name);

            if(user == null)
            {
                return NotFound(new { message = "Usuário não encontrado" });
            }

            article.user = user;
            var result =_articleService.Create(article);

            if(result != "Sucesso") {
                return NotFound(new { message = result });
            }

            return CreatedAtRoute("GetArticle", new { id = article.Id.ToString() }, article);
        }

        [HttpPut("{id:length(24)}")]
        [Authorize]
        public IActionResult Update(string id, Article articleIn)
        {
            var article = _articleService.Get(id);
            
            if (article == null)
            {
                return NotFound(new { message = "Usuário não encontrado" });
            }
            
            if( User.Identity.Name != article.user.Name)
            {
                return BadRequest(new { message = "O usuário não tem permissão para editar artigo de outro usuário" });
            }
    

            articleIn.Category_Id = _categoryService.GetIdCategoryByName(articleIn.Category_Id).Id;   
                
            _articleService.Update(id, articleIn);
            
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        [Authorize]
        public IActionResult Delete(string id)
        {
            var article = _articleService.Get(id);

            if (article == null)
            {
                return NotFound();
            }

            if(User.IsInRole("Student"))
            {
                if(User.Identity.Name != article.user.Name)
                {
                    return BadRequest(new { message = "O usuário não tem permissão para excuir artigo de outro usuário" });
                }
            }

            _articleService.Remove(article.Id);

            return NoContent();
        }
    }
}