using NewsSite.Core.DataTransferObjects.ArticleObjects.CommentObjects;
using NewsSite.Core.Domain.RepositoryContracts;
using NewsSite.Core.Helpers;
using NewsSite.Core.ServiceContracts.ArticlesCommentsContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.Services.ArticlesCommentsServices
{
    public class ArticlesCommentsAdderService : IArticlesCommentsAdderService
    {
        private readonly IArticlesCommentsRepository _articlesCommentsRepository;

        public ArticlesCommentsAdderService(IArticlesCommentsRepository articlesCommentsRepository)
        {
            _articlesCommentsRepository = articlesCommentsRepository;
        }

        public async Task<CommentResponse> AddComment(CommentAddRequest? commentAddRequest, Guid? userId)
        {
            if (commentAddRequest == null)
            {
                throw new ArgumentNullException("Comment add request can't be null.");
            }

            if (userId == null || userId == Guid.Empty)
            {
                throw new ArgumentNullException("User id can't be empty.");
            }

            ValidationHelper.ModelValidation(commentAddRequest);

            var addedComment = await _articlesCommentsRepository.AddCommentAsync(commentAddRequest.ToComment(userId.Value));
            return addedComment.ToCommentResponse();
        }
    }
}
