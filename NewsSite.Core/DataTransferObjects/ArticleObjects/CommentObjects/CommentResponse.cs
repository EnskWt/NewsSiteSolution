using NewsSite.Core.Domain.Models.ArticleModels;
using NewsSite.Core.Domain.Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.DataTransferObjects.ArticleObjects.CommentObjects
{
    public class CommentResponse
    {
        public Guid Id { get; set; }

        public Guid AuthorId { get; set; }
        public ApplicationUser Author { get; set; } = null!;

        public string Body { get; set; } = null!;
        public DateTime DatePublished { get; set; }
    }

    public static class CommentResponseExtensions
    {
        public static CommentResponse ToCommentResponse(this Comment comment)
        {
            return new CommentResponse
            {
                Id = comment.Id,
                AuthorId = comment.AuthorId,
                Author = comment.Author,
                Body = comment.Body,
                DatePublished = comment.DatePublished
            };
        }
    }
}
