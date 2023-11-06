using NewsSite.Core.Domain.Models.ArticleModels;
using NewsSite.Core.Enums.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.ServiceContracts.ArticlesContracts
{
    public interface IArticleExpressionsProvider
    {
        public Expression<Func<Article, bool>> GetFilter(SearchTerms searchTerm, string term);
        public Expression<Func<Article, object>> GetSort(SortAttributes sortAttribute);
    }
}
