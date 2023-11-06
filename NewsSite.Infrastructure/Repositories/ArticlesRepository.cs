using Microsoft.EntityFrameworkCore;
using NewsSite.Core.Domain.Models.ArticleModels;
using NewsSite.Core.Domain.RepositoryContracts;
using NewsSite.Infrastructure.DatabaseContext;
using System.Linq.Expressions;

namespace NewsSite.Infrastructure.Repositories
{
    public class ArticlesRepository : IArticlesRepository
    {
        private ApplicationDbContext _db;

        public ArticlesRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Article> AddArticleAsync(Article article)
        {
            await _db.AddAsync(article);
            await _db.SaveChangesAsync();

            return article;
        }

        public async Task<bool> DeleteArticleAsync(Guid id)
        {
            var article = await _db.Articles.FirstOrDefaultAsync(a => a.Id == id);

            if (article == null)
            {
                return false;
            }

            _db.Articles.Remove(article);
            await _db.SaveChangesAsync();

            return true; 
        }

        public async Task<Article?> GetArticleAsync(Guid id)
        {
            var article = await _db.Articles
                .Include(a => a.Author)
                .Include(a => a.Comments)
                    .ThenInclude(c => c.Author)
                .FirstOrDefaultAsync(a => a.Id == id);
            return article;
        }

        public async Task<List<Article>> GetArticlesAsync()
        {
            var articles = await _db.Articles
                .ToListAsync();
            return articles;
        }

        public async Task<List<Article>> GetFilteredArticlesAsync(Expression<Func<Article, bool>> filter)
        {
            var articles = await _db.Articles
                .Where(filter)
                .ToListAsync();
            return articles;
        }

        public async Task<Article> UpdateArticleAsync(Article article)
        {
            var matchingArticle = await _db.Articles.FirstOrDefaultAsync(a => a.Id == article.Id);
            if (matchingArticle == null)
            {
                return article;
            }

            matchingArticle.Title = article.Title;
            matchingArticle.Body = article.Body;
            matchingArticle.PreviewText = article.PreviewText;


            await _db.SaveChangesAsync();

            return article;
            
        }
    }
}
