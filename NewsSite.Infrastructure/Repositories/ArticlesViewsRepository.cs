using Microsoft.EntityFrameworkCore;
using NewsSite.Core.Domain.Models.ArticleModels;
using NewsSite.Core.Domain.RepositoryContracts;
using NewsSite.Infrastructure.DatabaseContext;

namespace NewsSite.Infrastructure.Repositories
{
    public class ArticlesViewsRepository : IArticlesViewsRepository
    {
        private ApplicationDbContext _db;

        public ArticlesViewsRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task IncrementArticleViewsAsync(Guid articleId, ViewLog newViewLog)
        {
            var article = await _db.Articles
                .Include(a => a.ViewLogs)
                .FirstOrDefaultAsync(a => a.Id == articleId);
            if (article == null)
            {
                return;
            }

            article.Views++;
            article.ViewLogs.Add(newViewLog);

            await _db.SaveChangesAsync();
        }

        public async Task<List<ViewLog>?> GetArticleViewLogsAsync(Guid articleId)
        {
            var article = await _db.Articles
                .Include(a => a.ViewLogs)
                .FirstOrDefaultAsync(a => a.Id == articleId);

            return article?.ViewLogs;
        }
    }
}
