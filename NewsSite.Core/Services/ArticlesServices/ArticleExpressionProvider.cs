using NewsSite.Core.Domain.Models.ArticleModels;
using NewsSite.Core.Enums.Application;
using NewsSite.Core.ServiceContracts.ArticlesContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.Services.ArticlesServices
{
    public class ArticleExpressionProvider : IArticleExpressionsProvider
    {
        private readonly Dictionary<SearchTerms, Func<string, Expression<Func<Article, bool>>>> _filters;

        private Dictionary<SortAttributes, Expression<Func<Article, object>>> _sorts;

        public ArticleExpressionProvider()
        {
            _filters = new Dictionary<SearchTerms, Func<string, Expression<Func<Article, bool>>>>
            {
                { SearchTerms.Author, (term) => a => a.AuthorId == Guid.Parse(term) }
            };

            _sorts = new Dictionary<SortAttributes, Expression<Func<Article, object>>>
            {
                { SortAttributes.Date, (Article a) => a.DatePublished },
                { SortAttributes.Views, (Article a) => a.Views }
            };
        }

        public Expression<Func<Article, bool>> GetFilter(SearchTerms searchTerm, string term)
        {
            return _filters[searchTerm](term);
        }

        public Expression<Func<Article, object>> GetSort(SortAttributes sortAttribute)
        {
            return _sorts[sortAttribute];
        }
    }
}
