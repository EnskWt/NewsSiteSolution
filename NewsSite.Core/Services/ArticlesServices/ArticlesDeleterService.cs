using NewsSite.Core.Domain.RepositoryContracts;
using NewsSite.Core.ServiceContracts.ArticlesContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.Services.ArticlesServices
{
    public class ArticlesDeleterService : IArticlesDeleterService
    {
        private IArticlesRepository _articlesRepository;

        public ArticlesDeleterService(IArticlesRepository articlesRepository)
        {
            _articlesRepository = articlesRepository;
        }

        public async Task<bool> DeleteArticle(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("Article id can't be null.");
            }

            //var article = await _articlesRepository.GetArticleAsync(id.Value);
            //if (article == null)
            //{
            //    return false;
            //}

            bool success = await _articlesRepository.DeleteArticleAsync(id.Value);
            return success;
        }
    }
}
