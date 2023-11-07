using NewsSite.Core.DataTransferObjects.ArticleObjects;
using NewsSite.Core.Domain.Models;
using NewsSite.Core.Enums.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.ServiceContracts.ArticlesContracts
{
    public interface IArticlesGetterService
    {
        /// <summary>
        /// Gets all articles from the database
        /// </summary>
        /// <returns>Returns list of articles</returns>
        Task<List<ArticleResponse>> GetArticles(SortAttributes sortAttribute = SortAttributes.Date, int count = 10);

        /// <summary>
        /// Gets all articles with a filter applied
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<List<ArticleResponse>> GetFilteredArticles(SearchTerms searchTerm, string value, SortAttributes sortAttribute = SortAttributes.Date, int count = 10);

        /// <summary>
        /// Gets a single article from the database by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns a single article</returns>
        Task<ArticleResponse?> GetArticle(Guid? id);
    }
}
