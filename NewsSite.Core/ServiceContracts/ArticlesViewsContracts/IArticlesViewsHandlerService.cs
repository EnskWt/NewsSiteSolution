using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.ServiceContracts.ArticlesViewsContracts
{
    public interface IArticlesViewsHandlerService
    {
        /// <summary>
        /// Increments the number of views for the article with the given id.
        /// </summary>
        /// <param name="articleId">Article id which views number should be increased.</param>
        /// <returns></returns>
        Task TryIncrementArticleViews(Guid articleId, string inputIpAddress);
    }
}
