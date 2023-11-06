using NewsSite.Core.Domain.Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.Domain.Models.ArticleModels
{
    public class Article
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        public string PreviewText { get; set; } = null!;
        public DateTime DatePublished { get; set; }

        public int Views { get; set; } = 0;
        public virtual List<ViewLog> ViewLogs { get; set; } = new List<ViewLog>();

        public virtual List<Comment> Comments { get; set; } = new List<Comment>();

        public Guid AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; } = null!;
    }
}
