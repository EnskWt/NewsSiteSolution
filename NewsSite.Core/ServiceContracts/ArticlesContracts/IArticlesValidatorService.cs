using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.ServiceContracts.ArticlesContracts
{
    public interface IArticlesValidatorService
    {
        Task<bool> IsArticleExist(Guid? articleId);

        Task<bool> IsArticleOwnedByAuthor(Guid? articleId, Guid? authorId);    
    }
}
