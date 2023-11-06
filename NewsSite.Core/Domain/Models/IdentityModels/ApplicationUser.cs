using Microsoft.AspNetCore.Identity;
using NewsSite.Core.Domain.Models.ArticleModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.Domain.Models.IdentityModels
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public virtual List<Article> Articles { get; set; } = new List<Article>();

        public virtual List<Comment> Comments { get; set; } = new List<Comment>();

    }
}
