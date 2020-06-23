namespace My_app_backend.Models
{
    public class ArticlestoreDatabaseSettings : IArticlestoreDatabaseSettings
    {
        public string ArticlesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IArticlestoreDatabaseSettings
    {
        string ArticlesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}