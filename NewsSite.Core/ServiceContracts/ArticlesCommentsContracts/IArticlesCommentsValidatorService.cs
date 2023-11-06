using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.ServiceContracts.ArticlesCommentsContracts
{
    public interface IArticlesCommentsValidatorService
    {
        Task<bool> IsCommentExist(Guid? articleId);

        Task<bool> IsCommentOwnedByAuthor(Guid? articleId, Guid? authorId);
    }
}
