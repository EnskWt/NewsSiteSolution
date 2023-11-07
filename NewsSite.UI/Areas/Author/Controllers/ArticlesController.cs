using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsSite.Core.DataTransferObjects.ArticleObjects;
using NewsSite.Core.Domain.Models.IdentityModels;
using NewsSite.Core.Enums.Application;
using NewsSite.Core.ServiceContracts.ArticlesContracts;
using NewsSite.UI.Filters.ActionFilters;
using System.ComponentModel;

namespace NewsSite.UI.Areas.Author.Controllers
{
    [Area("Author")]
    [Route("author/my-articles")]
    [Authorize(Roles = "Author")]
    public class ArticlesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IArticlesValidatorService _articlesValidatorService;

        private readonly IArticlesAdderService _articlesAdderService;
        private readonly IArticlesDeleterService _articlesDeleterService;
        private readonly IArticlesGetterService _articlesGetterService;
        private readonly IArticlesUpdaterService _articlesUpdaterService;

        public ArticlesController(UserManager<ApplicationUser> userManager, IArticlesValidatorService articlesValidatorService, IArticlesAdderService articlesAdderService, IArticlesDeleterService articlesDeleterService, IArticlesGetterService articlesGetterService, IArticlesUpdaterService articlesUpdaterService)
        {
            _userManager = userManager;

            _articlesValidatorService = articlesValidatorService;

            _articlesAdderService = articlesAdderService;
            _articlesDeleterService = articlesDeleterService;
            _articlesGetterService = articlesGetterService;
            _articlesUpdaterService = articlesUpdaterService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Articles");
            }

            var authorArticles = await _articlesGetterService.GetFilteredArticles(SearchTerms.Author, user.Id.ToString());

            return View(authorArticles);
        }

        [HttpGet]
        [Route("create")]
        public async Task<IActionResult> CreateArticle()
        {
            return View();
        }

        [HttpPost]
        [Route("create")]
        [TypeFilter(typeof(ArticleRequestModelStateCheckActionFilter))]
        public async Task<IActionResult> CreateArticle(ArticleAddRequest articleRequest)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Index", "Articles");
            }

            var articleResponse = await _articlesAdderService.AddArticle(articleRequest, user.Id);
            if (articleResponse == null)
            {
                return View(articleRequest);
            }

            return RedirectToAction("ArticleDetails", "Articles", new { id = articleResponse.Id });
        }

        [HttpGet]
        [Route("edit/{id}")]
        public async Task<IActionResult> UpdateArticle(Guid id)
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Articles");
            }

            var isArticleExistAndOwnedByAuthor = await _articlesValidatorService.IsArticleOwnedByAuthor(id, user.Id);

            if (!isArticleExistAndOwnedByAuthor)
            {
                return RedirectToAction("Index", "Articles");
            }

            var articleResponse = await _articlesGetterService.GetArticle(id);

            var articleRequest = articleResponse?.ToArticleUpdateRequest();
            return View(articleRequest);
        }

        [HttpPost]
        [Route("edit/{id}")]
        [TypeFilter(typeof(ArticleRequestModelStateCheckActionFilter))]
        public async Task<IActionResult> UpdateArticle(ArticleUpdateRequest articleRequest)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Articles");
            }

            var isArticleExistAndOwnedByAuthor = await _articlesValidatorService.IsArticleOwnedByAuthor(articleRequest.Id, user.Id);
            if (!isArticleExistAndOwnedByAuthor)
            {
                return RedirectToAction("Index", "Articles");
            }

            var updatedArticle = await _articlesUpdaterService.UpdateArticle(articleRequest);
            return RedirectToAction("ArticleDetails", "Articles", new { id = updatedArticle.Id });
        }

        [HttpGet]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteArticle(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Index", "Articles");
            }

            var isArticleExistAndOwnedByAuthor = await _articlesValidatorService.IsArticleOwnedByAuthor(id, user.Id);

            if (!isArticleExistAndOwnedByAuthor)
            {
                return RedirectToAction("Index", "Articles");
            }

            var articleResponse = await _articlesGetterService.GetArticle(id);

            return View(articleResponse);
        }

        [HttpPost]
        [Route("delete/{id}")]
        [DisplayName("DeleteArticle")]
        public async Task<IActionResult> DeleteArticlePost(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Index", "Articles");
            }

            var isArticleExistAndOwnedByAuthor = await _articlesValidatorService.IsArticleOwnedByAuthor(id, user.Id);

            if (!isArticleExistAndOwnedByAuthor)
            {
                return RedirectToAction("Index", "Articles");
            }           

            var success = await _articlesDeleterService.DeleteArticle(id);
            if (!success)
            {
                var articleResponse = await _articlesGetterService.GetArticle(id);
                return View(articleResponse);
            }

            return RedirectToAction("Index", "Articles");
        }
    }
}
