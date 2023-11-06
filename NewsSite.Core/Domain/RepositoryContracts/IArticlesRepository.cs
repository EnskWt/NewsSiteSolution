using NewsSite.Core.DataTransferObjects;
using NewsSite.Core.Domain.Models.ArticleModels;
using NewsSite.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.Domain.RepositoryContracts
{
    public interface IArticlesRepository
    {
        /// <summary>
        /// Add an article to the database.
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        Task<Article> AddArticleAsync(Article article);

        /// <summary>
        /// Delete an article from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteArticleAsync(Guid id);

        /// <summary>
        /// Gets all articles from the database
        /// </summary>
        /// <returns></returns>
        Task<List<Article>> GetArticlesAsync();

        Task<List<Article>> GetFilteredArticlesAsync(Expression<Func<Article, bool>> filter);

        /// <summary>
        /// Gets a single article from the database by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Article?> GetArticleAsync(Guid id);

        /// <summary>
        /// Update an article in the database.
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        Task<Article> UpdateArticleAsync(Article article);

    }
}
