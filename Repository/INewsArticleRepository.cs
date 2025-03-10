using Domain;

public interface INewsArticleRepository
{
    Task<IEnumerable<NewsArticle>> GetAllActiveNewsArticlesAsync();
    Task<NewsArticle> GetNewsArticleByIdAsync(string id);
    Task AddNewsArticleAsync(NewsArticle newsArticle);
    Task UpdateNewsArticleAsync(NewsArticle newsArticle);
    Task DeleteNewsArticleAsync(string id);
    Task UpdateTagsAsync(string newsArticleId, int[] selectedTags);
    Task<IEnumerable<NewsArticle>> SearchNewsArticlesAsync(string searchString); // Add this line
}
