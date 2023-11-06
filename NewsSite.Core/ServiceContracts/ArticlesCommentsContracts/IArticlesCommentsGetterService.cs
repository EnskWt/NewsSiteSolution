using NewsSite.Core.DataTransferObjects.ArticleObjects.CommentObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.ServiceContracts.ArticlesCommentsContracts
{
    public interface IArticlesCommentsGetterService
    {
        /// <summary>
        /// Returns all comments for an article
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns>Returns list of comment response data type</returns>
        Task<List<CommentResponse>> GetArticleComments(Guid? articleId, int count = 10);

        Task<CommentResponse> GetComment(Guid? commentId);
    }
}
