using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsSite.Core.DataTransferObjects.ArticleObjects.CommentObjects;
using NewsSite.Core.Domain.Models.IdentityModels;
using NewsSite.Core.ServiceContracts.ArticlesCommentsContracts;

namespace NewsSite.UI.Controllers
{
    [Route("comments")]
    public class CommentsController : Controller
    {
        private readonly IArticlesCommentsAdderService _articlesCommentsAdderService;
        private readonly IArticlesCommentsDeleterService _articlesCommentsDeleterService;

        private readonly IArticlesCommentsValidatorService _articlesCommentsValidatorService;

        private readonly UserManager<ApplicationUser> _userManager;

        public CommentsController(IArticlesCommentsAdderService articlesCommentsAdderService, IArticlesCommentsDeleterService articlesCommentsDeleterService, IArticlesCommentsValidatorService articlesCommentsValidatorService, UserManager<ApplicationUser> userManager)
        {
            _articlesCommentsAdderService = articlesCommentsAdderService;
            _articlesCommentsDeleterService = articlesCommentsDeleterService;

            _articlesCommentsValidatorService = articlesCommentsValidatorService;

            _userManager = userManager;
        }

        [HttpPost]
        [Route("add-comment")]
        public async Task<IActionResult> AddComment(CommentAddRequest commentAddRequest)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("ArticleDetails", "Articles", new { id = commentAddRequest.ArticleId });
            }

            await _articlesCommentsAdderService.AddComment(commentAddRequest, user.Id);

            return RedirectToAction("ArticleDetails", "Articles", new { id = commentAddRequest.ArticleId });
        }

        [HttpGet]
        [Route("delete-comment/{id}")]
        public async Task<IActionResult> DeleteComment(Guid id, Guid returnArticle)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Index", "Articles");
            }

            var isCommentExistAndOwnedByAuthor = await _articlesCommentsValidatorService.IsCommentOwnedByAuthor(id, user.Id);
            if (!isCommentExistAndOwnedByAuthor)
            {
                return RedirectToAction("ArticleDetails", "Articles", new { id = returnArticle });
            }

            await _articlesCommentsDeleterService.DeleteComment(id);

            return RedirectToAction("ArticleDetails", "Articles", new { id = returnArticle });
        }
    }
}
