using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.ServiceContracts.ArticlesCommentsContracts
{
    public interface IArticlesCommentsDeleterService
    {
        /// <summary>
        /// Delete comment by id
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        Task<bool> DeleteComment(Guid? commentId);
    }
}
