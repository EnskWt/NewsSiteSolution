using NewsSite.Core.DataTransferObjects.ArticleObjects.CommentObjects;
using NewsSite.Core.Domain.Models.ArticleModels;
using NewsSite.Core.Domain.Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.DataTransferObjects.ArticleObjects
{
    public class ArticleResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        public string PreviewText { get; set; } = null!;
        public DateTime DatePublished { get; set; }

        public int Views { get; set; } = 0;

        public virtual List<CommentResponse> Comments { get; set; } = null!;

        public Guid AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; } = null!;
    }

    public static class ArticleResponseExtensions
    {
        public static ArticleResponse ToArticleResponse(this Article article)
        {
            return new ArticleResponse
            {
                Id = article.Id,
                Title = article.Title,
                Body = article.Body,
                PreviewText = article.PreviewText,
                DatePublished = article.DatePublished,
                Views = article.Views,
                Comments = article.Comments.Select(c => c.ToCommentResponse()).ToList(),
                AuthorId = article.AuthorId,
                Author = article.Author
            };
        }


    }
}
