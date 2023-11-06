using NewsSite.Core.Domain.Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.Domain.Models.ArticleModels
{
    public class Comment
    {
        public Guid Id { get; set; }

        public string Body { get; set; } = null!;
        public DateTime DatePublished { get; set; } = DateTime.UtcNow;

        public Guid AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; } = null!;

        public Guid ArticleId { get; set; }
        public virtual Article Article { get; set; } = null!;
    }
}
