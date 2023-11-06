using NewsSite.Core.Domain.Models;
using NewsSite.Core.Domain.Models.ArticleModels;
using NewsSite.Core.Domain.RepositoryContracts;
using NewsSite.Core.ServiceContracts.ArticlesContracts;
using NewsSite.Core.ServiceContracts.ArticlesViewsContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.Services.ArticlesViewsServices
{
    public class ArticlesViewsHandlerService : IArticlesViewsHandlerService
    {
        private readonly IArticlesViewsRepository _articlesViewsRepository;

        public ArticlesViewsHandlerService(IArticlesViewsRepository articlesViewsRepository)
        {
            _articlesViewsRepository = articlesViewsRepository;
        }

        public async Task TryIncrementArticleViews(Guid articleId, string inputIpAddress)
        {
            var viewLogs = await _articlesViewsRepository.GetArticleViewLogsAsync(articleId);

            if (viewLogs == null)
            {
                return;
            }

            if (!viewLogs.Any(vl => vl.IpAddress == inputIpAddress))
            {
                var newViewLog = new ViewLog
                {
                    IpAddress = inputIpAddress,
                    ArticleId = articleId
                };
                await _articlesViewsRepository.IncrementArticleViewsAsync(articleId, newViewLog);
            }
        }
    }
}
