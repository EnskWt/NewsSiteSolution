using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsSite.Core.DataTransferObjects.ArticleObjects.CommentObjects;
using NewsSite.Core.Domain.Models.ArticleModels;
using NewsSite.Core.Enums.Application;
using NewsSite.Core.ServiceContracts.ArticlesCommentsContracts;
using NewsSite.Core.ServiceContracts.ArticlesContracts;
using NewsSite.Core.ServiceContracts.ArticlesViewsContracts;
using NewsSite.UI.Filters.ActionFilters;

namespace NewsSite.UI.Controllers
{
    [AllowAnonymous]
    public class ArticlesController : Controller
    {
        private readonly IArticlesCommentGetterService _articlesGetterService;

        private readonly IArticlesCommentsGetterService _articlesCommentsGetterService;

        public ArticlesController(IArticlesCommentGetterService articlesGetterService, IArticlesCommentsGetterService articlesCommentsGetterService)
        {
            _articlesGetterService = articlesGetterService;
            _articlesCommentsGetterService = articlesCommentsGetterService;
        }

        [HttpGet]
        [Route("/")]
        [Route("articles")]
        [TypeFilter(typeof(SortOptionsActionFilter))]
        public async Task<IActionResult> Index([FromQuery] SortAttributes sortBy = SortAttributes.Date)
        {
            var articles = await _articlesGetterService.GetArticles(sortBy);

            return View(articles);
        }

        [HttpGet]
        [Route("articles/{id}")]
        [TypeFilter(typeof(ArticlesViewsHandlerActionFilter))]
        [TypeFilter(typeof(ArticleCommentAddRequestActionFilter))]
        public async Task<IActionResult> ArticleDetails(Guid id)
        {
            var article = await _articlesGetterService.GetArticle(id);

            if (article == null)
            {
                return RedirectToAction("Index", "Articles");
            }

            return View(article);
        }
    }
}
