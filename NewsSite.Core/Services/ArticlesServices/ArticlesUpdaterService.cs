using NewsSite.Core.DataTransferObjects.ArticleObjects;
using NewsSite.Core.Domain.RepositoryContracts;
using NewsSite.Core.Helpers;
using NewsSite.Core.ServiceContracts.ArticlesContracts;

namespace NewsSite.Core.Services.ArticlesServices
{
    public class ArticlesUpdaterService : IArticlesUpdaterService
    {
        private IArticlesRepository _articlesRepository;

        public ArticlesUpdaterService(IArticlesRepository articlesRepository)
        {
            _articlesRepository = articlesRepository;
        }

        public async Task<ArticleResponse> UpdateArticle(ArticleUpdateRequest? articleRequest)
        {
            if (articleRequest == null)
            {
                throw new ArgumentNullException("Article request can't be null.");
            }

            ValidationHelper.ModelValidation(articleRequest); // should throw exception 'ArgumentException' if model is not valid

            var updatedArticle = await _articlesRepository.UpdateArticleAsync(articleRequest.ToArticle());
            return updatedArticle.ToArticleResponse();
        }
    }
}
