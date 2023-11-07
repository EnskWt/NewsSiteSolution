using NewsSite.Core.DataTransferObjects.ArticleObjects;
using NewsSite.Core.Domain.Models.ArticleModels;
using NewsSite.Core.Domain.RepositoryContracts;
using NewsSite.Core.Enums.Application;
using NewsSite.Core.ServiceContracts.ArticlesContracts;
using System.Linq;

namespace NewsSite.Core.Services.ArticlesServices
{
    public class ArticlesGetterService : IArticlesGetterService
    {
        private IArticlesRepository _articlesRepository;

        private IArticleExpressionsProvider _articleExpressionsProvider;

        public ArticlesGetterService(IArticlesRepository articlesRepository, IArticleExpressionsProvider articleExpressionsProvider)
        {
            _articlesRepository = articlesRepository;
            _articleExpressionsProvider = articleExpressionsProvider;
        }

        public async Task<List<ArticleResponse>> GetArticles(SortAttributes sortAttribute = SortAttributes.Date, int count = 10)
        {
            List<Article> articles = await _articlesRepository.GetArticlesAsync();
            return articles.AsQueryable()
                .OrderByDescending(_articleExpressionsProvider.GetSort(sortAttribute))
                .Take(count)
                .Select(a => a.ToArticleResponse())
                .ToList();
        }

        public async Task<List<ArticleResponse>> GetFilteredArticles(SearchTerms searchTerm, string term, SortAttributes sortAttribute = SortAttributes.Date, int count = 10)
        {
            List<Article> articles = await _articlesRepository.GetFilteredArticlesAsync(_articleExpressionsProvider.GetFilter(searchTerm, term));
            return articles.AsQueryable()
                .OrderByDescending(_articleExpressionsProvider.GetSort(sortAttribute))
                .Take(count)
                .Select(a => a.ToArticleResponse())
                .ToList();
        }

        public async Task<ArticleResponse?> GetArticle(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("Article not found.");
            }

            Article? article = await _articlesRepository.GetArticleAsync(id.Value);            

            return article?.ToArticleResponse();
        }
    }
}
