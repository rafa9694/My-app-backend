using My_app_backend.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace My_app_backend.Services
{
    public class CategoryService
    {
        private readonly IMongoCollection<Category> _categories;

        public CategoryService(ICategorystoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _categories = database.GetCollection<Category>(settings.CategoriesCollectionName);
        }

        public List<Category> Get() =>
            _categories.Find(book => true).ToList();

        public Category Get(string id) =>
            _categories.Find<Category>(category => category.Id == id).FirstOrDefault();
        public Category Create(Category category)
        {
            _categories.InsertOne(category);
            return category;
        }

        public void Update(string id, Category categoryIn) =>
            _categories.ReplaceOne(category => category.Id == id, categoryIn);

        public void Remove(Category categoryIn) =>
            _categories.DeleteOne(category => category.Id == categoryIn.Id);

        public void Remove(string id) => 
            _categories.DeleteOne(category => category.Id == id);

        
        public bool ExistSubCategories(string categoryName)
        {
            var category = _categories.Find<Category>(category => 
                category.Path.Contains(categoryName) && category.Name != categoryName).FirstOrDefault();
            return category != null;
        }

        public bool ExistCategoryName(string categoryName)
        {
           var category = _categories.Find<Category>(category => category.Name == categoryName).FirstOrDefault();
           return category !=null;
        }    
    }
}