using NewsSite.Core.DataTransferObjects.ArticleObjects.CommentObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.ServiceContracts.ArticlesCommentsContracts
{
    public interface IArticlesCommentsAdderService
    {
        /// <summary>
        /// Add comment to database with relation to article
        /// </summary>
        /// <param name="commentAddRequest">Comment add request that represents comment to be added</param>
        /// <returns>Return comment response object</returns>
        Task<CommentResponse> AddComment(CommentAddRequest? commentAddRequest, Guid? userId);
    }
}
