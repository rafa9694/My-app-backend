namespace My_app_backend.Models
{
    public class CategorystoreDatabaseSettings : ICategorystoreDatabaseSettings
    {
        public string CategoriesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface ICategorystoreDatabaseSettings
    {
        string CategoriesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}