using NewsSite.Core.Domain.Models.ArticleModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.DataTransferObjects.ArticleObjects
{
    public class ArticleAddRequest
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;
        [Required]
        [StringLength(maximumLength: 600, MinimumLength = 300)]
        public string Body { get; set; } = null!;

        public Article ToArticle(Guid authorId)
        {
            return new Article
            {
                Title = Title,
                Body = Body,
                PreviewText = $"{Body.Substring(0, 100)}...",
                DatePublished = DateTime.Now,
                AuthorId = authorId
            };
        }
    }
}
