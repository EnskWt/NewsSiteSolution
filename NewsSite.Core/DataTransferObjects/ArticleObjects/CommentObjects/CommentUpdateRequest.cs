using NewsSite.Core.Domain.Models.ArticleModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.DataTransferObjects.ArticleObjects.CommentObjects
{
    public class CommentUpdateRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(maximumLength: 150, MinimumLength = 5)]
        public string Body { get; set; } = null!;

        public Comment ToComment()
        {
            return new Comment
            {
                Id = Id,
                Body = Body,
            };
        }
    }
}
