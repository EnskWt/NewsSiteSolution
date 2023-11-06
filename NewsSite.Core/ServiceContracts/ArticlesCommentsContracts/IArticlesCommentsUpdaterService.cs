using NewsSite.Core.DataTransferObjects.ArticleObjects.CommentObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.ServiceContracts.ArticlesCommentsContracts
{
    public interface IArticlesCommentsUpdaterService
    {
        /// <summary>
        /// Update comment in database
        /// </summary>
        /// <param name="commentUpdateRequest">Updated version of comment</param>
        /// <returns>Returns comment response type object</returns>
        Task<CommentResponse> UpdateComment(CommentUpdateRequest? commentUpdateRequest);
    }
}
