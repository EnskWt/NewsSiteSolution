using NewsSite.Core.ServiceContracts.ArticlesCommentsContracts;
using NewsSite.Core.ServiceContracts.ArticlesContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.Services.ArticlesCommentsServices
{
    public class ArticlesCommentsValidatorService : IArticlesCommentsValidatorService
    {
        private readonly IArticlesCommentsGetterService _articlesCommentsGetterService;

        public ArticlesCommentsValidatorService(IArticlesCommentsGetterService articlesCommentGetterService)
        {
            _articlesCommentsGetterService = articlesCommentGetterService;
        }

        public async Task<bool> IsCommentExist(Guid? commentId)
        {
            var comment = await _articlesCommentsGetterService.GetComment(commentId);

            return comment != null;
        }

        public async Task<bool> IsCommentOwnedByAuthor(Guid? commentId, Guid? authorId)
        {
            var comment = await _articlesCommentsGetterService.GetComment(commentId);

            if (comment == null)
            {
                return false;
            }

            return comment.AuthorId == authorId;
        }
    }
}
