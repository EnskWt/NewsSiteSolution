using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewsSite.Core.Domain.Models.ArticleModels;
using NewsSite.Core.Domain.Models.IdentityModels;

namespace NewsSite.Infrastructure.DatabaseContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public virtual DbSet<Article> Articles { get; set; } = null!;

        public virtual DbSet<Comment> Comments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Article entity configuration

            modelBuilder.Entity<Article>()
                .HasOne(a => a.Author)
                .WithMany(au => au.Articles)
                .HasForeignKey(a => a.AuthorId);

            modelBuilder.Entity<Article>()
                .HasMany(a => a.ViewLogs)
                .WithOne(v => v.Article)
                .HasForeignKey(v => v.ArticleId);

            modelBuilder.Entity<Article>()
                .HasMany(a => a.Comments)
                .WithOne(c => c.Article)
                .HasForeignKey(c => c.ArticleId);

            modelBuilder.Entity<Article>()
                .HasOne(a => a.Author)
                .WithMany(au => au.Articles)
                .HasForeignKey(a => a.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Comment entity configuration

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Author)
                .WithMany(au => au.Comments)
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion
        }
    }
}
