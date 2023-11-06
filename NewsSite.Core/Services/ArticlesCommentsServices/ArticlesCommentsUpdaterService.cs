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
    public class ArticlesCommentsUpdaterService : IArticlesCommentsUpdaterService
    {
        private readonly IArticlesCommentsRepository _articlesCommentsRepository;

        public ArticlesCommentsUpdaterService(IArticlesCommentsRepository articlesCommentsRepository)
        {
            _articlesCommentsRepository = articlesCommentsRepository;
        }

        public async Task<CommentResponse> UpdateComment(CommentUpdateRequest? commentUpdateRequest)
        {
            if (commentUpdateRequest == null)
            {
                throw new ArgumentNullException("Comment update request can't be null.");
            }

            ValidationHelper.ModelValidation(commentUpdateRequest);

            var updatedComment = await _articlesCommentsRepository.UpdateCommentAsync(commentUpdateRequest.ToComment());
            return updatedComment.ToCommentResponse();
        }
    }
}
