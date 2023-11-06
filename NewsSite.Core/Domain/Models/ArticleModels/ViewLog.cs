using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.Domain.Models.ArticleModels
{
    public class ViewLog
    {
        public Guid Id { get; set; }
        public string IpAddress { get; set; } = null!;
        public DateTime DateViewed { get; set; } = DateTime.UtcNow;

        public Guid ArticleId { get; set; }
        public virtual Article Article { get; set; } = null!;
    }
}
