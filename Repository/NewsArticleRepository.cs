using Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class NewsArticleRepository : INewsArticleRepository
    {
        private readonly FunewsManagementContext _context;

        public NewsArticleRepository(FunewsManagementContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<NewsArticle>> SearchNewsArticlesAsync(string searchString)
        {
            var query = _context.NewsArticles.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(n => n.NewsTitle.Contains(searchString) || n.Headline.Contains(searchString));
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetAllActiveNewsArticlesAsync()
        {
            return await _context.NewsArticles.Where(n => n.NewsStatus == true).ToListAsync();
        }

        public async Task<NewsArticle> GetNewsArticleByIdAsync(string id)
        {
            return await _context.NewsArticles
                .Include(n => n.Tags)
                .Include(n => n.Category)
                .FirstOrDefaultAsync(n => n.NewsArticleId == id);
        }

        public async Task AddNewsArticleAsync(NewsArticle newsArticle)
        {
            await _context.NewsArticles.AddAsync(newsArticle);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateNewsArticleAsync(NewsArticle newsArticle)
        {
            _context.NewsArticles.Update(newsArticle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNewsArticleAsync(string id)
        {
            var newsArticle = await _context.NewsArticles.FindAsync(id);
            if (newsArticle != null)
            {
                _context.NewsArticles.Remove(newsArticle);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateTagsAsync(string newsArticleId, int[] selectedTags)
        {
            var newsArticle = await _context.NewsArticles
                .Include(n => n.Tags)
                .FirstOrDefaultAsync(n => n.NewsArticleId == newsArticleId);

            if (newsArticle != null)
            {
                newsArticle.Tags.Clear();
                foreach (var tagId in selectedTags)
                {
                    var tag = await _context.Tags.FindAsync(tagId);
                    if (tag != null)
                    {
                        newsArticle.Tags.Add(tag);
                    }
                }
                await _context.SaveChangesAsync();
            }
        }
    }
}
