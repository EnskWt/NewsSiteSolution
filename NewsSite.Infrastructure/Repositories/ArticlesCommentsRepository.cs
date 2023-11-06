using Microsoft.EntityFrameworkCore;
using NewsSite.Core.Domain.Models.ArticleModels;
using NewsSite.Core.Domain.RepositoryContracts;
using NewsSite.Infrastructure.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Infrastructure.Repositories
{
    public class ArticlesCommentsRepository : IArticlesCommentsRepository
    {
        private readonly ApplicationDbContext _db;

        public ArticlesCommentsRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Comment> AddCommentAsync(Comment comment)
        {
            await _db.Comments.AddAsync(comment);
            await _db.SaveChangesAsync();

            return comment;
        }

        public async Task<bool> DeleteCommentAsync(Guid commentId)
        {
            var comment = await _db.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null)
            {
                return false;
            }

            _db.Comments.Remove(comment);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<List<Comment>> GetArticleCommentsAsync(Guid articleId)
        {
            var comments = await _db.Comments
                .Include(c => c.Author)
                .Where(c => c.ArticleId == articleId)
                .ToListAsync();

            return comments;
            
        }

        public async Task<Comment?> GetCommentAsync(Guid commentId)
        {
            var comment = await _db.Comments
                .Include(c => c.Author)
                .FirstOrDefaultAsync(c => c.Id == commentId);

            return comment;
        }

        public async Task<Comment> UpdateCommentAsync(Comment comment)
        {
            var matchingComment = await _db.Comments.FirstOrDefaultAsync(c => c.Id == comment.Id);
            if (matchingComment == null)
            {
                return comment;
            }

            matchingComment.Body = comment.Body;

            await _db.SaveChangesAsync();

            return matchingComment;
        }
    }
}
