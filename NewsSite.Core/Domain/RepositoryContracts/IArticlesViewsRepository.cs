using NewsSite.Core.Domain.Models.ArticleModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.Domain.RepositoryContracts
{
    public interface IArticlesViewsRepository
    {
        /// <summary>
        /// Increment article views.  
        /// </summary>
        /// <param name="articleId">Article id</param>
        /// <returns></returns>
        Task IncrementArticleViewsAsync(Guid articleId, ViewLog newViewLog);

        /// <summary>
        /// Return article view logs.
        /// </summary>
        /// <param name="articleId">Id of article</param>
        /// <returns>Return object of List<ViewLog>> type</returns>
        Task<List<ViewLog>?> GetArticleViewLogsAsync(Guid articleId);
    }
}
