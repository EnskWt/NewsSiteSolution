using NewsSite.Core.DataTransferObjects.ArticleObjects;
using NewsSite.Core.Domain.RepositoryContracts;
using NewsSite.Core.Helpers;
using NewsSite.Core.ServiceContracts.ArticlesContracts;

namespace NewsSite.Core.Services.ArticlesServices
{
    public class ArticlesAdderService : IArticlesAdderService
    {
        private IArticlesRepository _articlesRepository;

        public ArticlesAdderService(IArticlesRepository articlesRepository)
        {
            _articlesRepository = articlesRepository;
        }

        public async Task<ArticleResponse?> AddArticle(ArticleAddRequest? articleRequest, Guid? userId)
        {
            if (articleRequest == null)
            {
                throw new ArgumentNullException("Article request can't be null.");
            }

            if (userId == null || userId == Guid.Empty)
            {
                throw new ArgumentNullException("User id can't be empty.");
            }

            ValidationHelper.ModelValidation(articleRequest); // should throw exception 'ArgumentException' if model is not valid

            var addedArticle = await _articlesRepository.AddArticleAsync(articleRequest.ToArticle(userId.Value));
            return addedArticle.ToArticleResponse();
        }
    }
}
