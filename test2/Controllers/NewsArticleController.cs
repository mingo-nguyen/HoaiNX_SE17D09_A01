using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repository;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HoaiMVC.Controllers
{
    public class NewsArticleController : Controller
    {
        private readonly INewsArticleRepository _newsArticleRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<NewsArticleController> _logger;

        public NewsArticleController(INewsArticleRepository newsArticleRepository, ITagRepository tagRepository, ICategoryRepository categoryRepository, ILogger<NewsArticleController> logger)
        {
            _newsArticleRepository = newsArticleRepository;
            _tagRepository = tagRepository;
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var newsArticles = await _newsArticleRepository.SearchNewsArticlesAsync(searchString);

            return View(newsArticles);
        }



        public async Task<IActionResult> Details(string id)
        {
            var newsArticle = await _newsArticleRepository.GetNewsArticleByIdAsync(id);
            if (newsArticle == null)
            {
                return NotFound();
            }
            return View(newsArticle);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Tags = await GetTagsSelectList();
            ViewBag.Categories = await GetCategoriesSelectList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsArticle newsArticle, int[] Tags)
        {
            // Generate a new Guid for the NewsArticleId before checking ModelState
            newsArticle.NewsArticleId = Guid.NewGuid().ToString();
            _logger.LogWarning(newsArticle.NewsArticleId);
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Model state is valid. Adding news article to the database.");
                await _newsArticleRepository.AddNewsArticleAsync(newsArticle);
                await _newsArticleRepository.UpdateTagsAsync(newsArticle.NewsArticleId, Tags);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _logger.LogWarning("Model state is invalid.");
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        _logger.LogError($"Property: {state.Key}, Error: {error.ErrorMessage}");
                    }
                }
            }
            ViewBag.Tags = await GetTagsSelectList();
            ViewBag.Categories = await GetCategoriesSelectList();
            return View(newsArticle);
        }





        public async Task<IActionResult> Edit(string id)
        {
            var newsArticle = await _newsArticleRepository.GetNewsArticleByIdAsync(id);
            if (newsArticle == null)
            {
                return NotFound();
            }
            ViewBag.Tags = await GetTagsSelectList();
            ViewBag.Categories = await GetCategoriesSelectList();
            return View(newsArticle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, NewsArticle newsArticle, int[] selectedTags)
        {
            if (id != newsArticle.NewsArticleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _newsArticleRepository.UpdateNewsArticleAsync(newsArticle);
                await _newsArticleRepository.UpdateTagsAsync(newsArticle.NewsArticleId, selectedTags);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Tags = await GetTagsSelectList();
            ViewBag.Categories = await GetCategoriesSelectList();
            return View(newsArticle);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var newsArticle = await _newsArticleRepository.GetNewsArticleByIdAsync(id);
            if (newsArticle == null)
            {
                return NotFound();
            }
            return View(newsArticle);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _newsArticleRepository.DeleteNewsArticleAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetTagsSelectList()
        {
            var tags = await _tagRepository.GetAllTagsAsync();
            return tags.Select(tag => new SelectListItem
            {
                Value = tag.TagId.ToString(),
                Text = tag.TagName
            });
        }

        private async Task<IEnumerable<SelectListItem>> GetCategoriesSelectList()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return categories.Select(category => new SelectListItem
            {
                Value = category.CategoryId.ToString(),
                Text = category.CategoryName
            });
        }
    }
}
