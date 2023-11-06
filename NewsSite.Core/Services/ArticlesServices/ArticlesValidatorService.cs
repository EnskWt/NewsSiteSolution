using NewsSite.Core.DataTransferObjects.ArticleObjects;
using NewsSite.Core.ServiceContracts.ArticlesContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.Services.ArticlesServices
{
    public class ArticlesValidatorService : IArticlesValidatorService
    {
        private IArticlesCommentGetterService _articlesGetterService;

        public ArticlesValidatorService(IArticlesCommentGetterService articlesGetterService)
        {
            _articlesGetterService = articlesGetterService;
        }

        public async Task<bool> IsArticleExist(Guid? articleId)
        {
            var article = await _articlesGetterService.GetArticle(articleId);

            return article != null;
        }

        public async Task<bool> IsArticleOwnedByAuthor(Guid? articleId, Guid? authorId)
        {
            var article = await _articlesGetterService.GetArticle(articleId);

            if (article == null)
            {
                return false;
            }
            
            return article.AuthorId == authorId;
        }
    }
}
