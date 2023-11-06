using NewsSite.Core.DataTransferObjects.ArticleObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.ServiceContracts.ArticlesContracts
{
    public interface IArticlesUpdaterService
    {
        /// <summary>
        /// Update an article in the database.
        /// </summary>
        /// <param name="articleRequest">A new article object with updated data</param>
        /// <returns></returns>
        Task<ArticleResponse> UpdateArticle(ArticleUpdateRequest? articleRequest);
    }
}
