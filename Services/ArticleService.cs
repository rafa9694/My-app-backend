using My_app_backend.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace My_app_backend.Services
{
    public class ArticleService
    {
        private readonly IMongoCollection<Article> _articles;
        private readonly IMongoCollection<Category> _categories;

        public ArticleService(IArticlestoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _articles = database.GetCollection<Article>(settings.ArticlesCollectionName);
            _categories = database.GetCollection<Category>("Category");
        }

        public List<Article> Get() =>
            _articles.Find(article => true).ToList();

        public Article Get(string id) =>
            _articles.Find<Article>(article => article.Id == id).FirstOrDefault();

        public Article GetArticleByCategory(string categoryId) =>
            _articles.Find<Article>(article => article.Category_Id == categoryId).FirstOrDefault();
        public string Create(Article article)
        {
            var category = _categories.Find<Category>(category => category.Name == article.Category_Id)
                .FirstOrDefault();
            if(category == null) { 
                return $"Não foi possível localziar a Categoria {article.Category_Id}";
            }
            article.Category_Id = category.Id;   
            _articles.InsertOne(article);
            return "Sucesso";
            
        }

        public void Update(string id, Article articleIn) =>
            _articles.ReplaceOne(article => article.Id == id, articleIn);

        public void Remove(Article articleIn) =>
            _articles.DeleteOne(article => article.Id == articleIn.Id);

        public void Remove(string id) => 
            _articles.DeleteOne(article => article.Id == id);
    }
}