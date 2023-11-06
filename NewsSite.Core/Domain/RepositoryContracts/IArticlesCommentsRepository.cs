using NewsSite.Core.Domain.Models.ArticleModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.Domain.RepositoryContracts
{
    public interface IArticlesCommentsRepository
    {
        /// <summary>
        /// Add coment to database
        /// </summary>
        /// <param name="comment">Comment domain model to be added as comment</param>
        /// <returns>Return comment entity model</returns>
        Task<Comment> AddCommentAsync(Comment comment);

        /// <summary>
        /// Update an existed comment in database
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        Task<Comment> UpdateCommentAsync(Comment comment);

        /// <summary>
        /// Delete comment from database
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        Task<bool> DeleteCommentAsync(Guid commentId);

        /// <summary>
        /// Return all article comments from database
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        Task<List<Comment>> GetArticleCommentsAsync(Guid articleId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        Task<Comment?> GetCommentAsync(Guid commentId);
    }
}
