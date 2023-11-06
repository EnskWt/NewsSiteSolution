using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.ServiceContracts.ArticlesContracts
{
    public interface IArticlesDeleterService
    {
        /// <summary>
        /// Delete an article from the database.
        /// </summary>
        /// <param name="id">Id of the article that should be deleted</param>
        /// <returns></returns>
        Task<bool> DeleteArticle(Guid? id);
    }
}
