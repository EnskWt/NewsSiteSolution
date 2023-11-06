using Microsoft.AspNetCore.Mvc.Filters;
using NewsSite.Core.ServiceContracts.ArticlesViewsContracts;

namespace NewsSite.UI.Filters.ActionFilters
{
    public class ArticlesViewsHandlerActionFilter : IAsyncActionFilter
    {
        private readonly IArticlesViewsHandlerService _articlesViewsHandlerService;

        public ArticlesViewsHandlerActionFilter(IArticlesViewsHandlerService articlesViewsHandlerService)
        {
            _articlesViewsHandlerService = articlesViewsHandlerService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // before logic

            if (context.ActionArguments.ContainsKey("id"))
            {
                var articleId = context.ActionArguments["id"] as Guid?;
                if (articleId.HasValue)
                {
                    await _articlesViewsHandlerService.TryIncrementArticleViews(articleId.Value, context.HttpContext.Connection.RemoteIpAddress!.ToString());
                }           
            }

            await next();

            // after logic
        }
    }
}
