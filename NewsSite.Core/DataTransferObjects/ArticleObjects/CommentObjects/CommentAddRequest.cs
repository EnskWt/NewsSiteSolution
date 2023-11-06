using NewsSite.Core.Domain.Models.ArticleModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.DataTransferObjects.ArticleObjects.CommentObjects
{
    public class CommentAddRequest
    {
        [Required]
        [StringLength(maximumLength: 150)]
        public string Body { get; set; } = null!;

        [Required]
        public DateTime DatePublished { get; set; } = DateTime.UtcNow;

        [Required]
        public Guid ArticleId { get; set; }

        public Comment ToComment(Guid authorId)
        {
            return new Comment
            {
                Body = Body,
                DatePublished = DatePublished,
                ArticleId = ArticleId,
                AuthorId = authorId
            };
        }
    }
}
