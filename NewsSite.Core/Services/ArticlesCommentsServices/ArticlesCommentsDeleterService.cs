using NewsSite.Core.Domain.RepositoryContracts;
using NewsSite.Core.ServiceContracts.ArticlesCommentsContracts;

namespace NewsSite.Core.Services.ArticlesCommentsServices
{
    public class ArticlesCommentsDeleterService : IArticlesCommentsDeleterService
    {
        private readonly IArticlesCommentsRepository _articlesCommentsRepository;

        public ArticlesCommentsDeleterService(IArticlesCommentsRepository articlesCommentsRepository)
        {
            _articlesCommentsRepository = articlesCommentsRepository;
        }

        public async Task<bool> DeleteComment(Guid? commentId)
        {
            if (commentId == null || commentId == Guid.Empty)
            {
                throw new ArgumentNullException("Article id can't be null.");
            }

            var success = await _articlesCommentsRepository.DeleteCommentAsync(commentId.Value);

            return success;
        }
    }
}
