using NewsSite.Core.DataTransferObjects.ArticleObjects.CommentObjects;
using NewsSite.Core.Domain.Models.ArticleModels;
using NewsSite.Core.Domain.RepositoryContracts;
using NewsSite.Core.ServiceContracts.ArticlesCommentsContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.Services.ArticlesCommentsServices
{
    public class ArticlesCommentsGetterService : IArticlesCommentsGetterService
    {
        private readonly IArticlesCommentsRepository _articlesCommentsRepository;

        public ArticlesCommentsGetterService(IArticlesCommentsRepository articlesCommentsRepository)
        {
            _articlesCommentsRepository = articlesCommentsRepository;
        }

        public async Task<List<CommentResponse>> GetArticleComments(Guid? articleId, int count = 10)
        {
            if (articleId == null || articleId == Guid.Empty)
            {
                throw new ArgumentNullException("Article id can't be null.");
            }

            List<Comment> comments = await _articlesCommentsRepository.GetArticleCommentsAsync(articleId.Value);
            return comments
                .OrderByDescending(c => c.DatePublished)
                .Take(count)
                .Select(c => c.ToCommentResponse())
                .ToList();
        }

        public async Task<CommentResponse?> GetComment(Guid? commentId)
        {
            if (commentId == null || commentId == Guid.Empty)
            {
                throw new ArgumentNullException("Comment id can't be null.");
            }

            var comment = await _articlesCommentsRepository.GetCommentAsync(commentId.Value);

            return comment?.ToCommentResponse();
        }
    }
}
