using NewsSite.Core.DataTransferObjects.ArticleObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.ServiceContracts.ArticlesContracts
{
    public interface IArticlesAdderService
    {
        /// <summary>
        /// Add article to database
        /// </summary>
        /// <param name="articleRequest"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ArticleResponse?> AddArticle(ArticleAddRequest? articleRequest, Guid? userId);
    }
}
