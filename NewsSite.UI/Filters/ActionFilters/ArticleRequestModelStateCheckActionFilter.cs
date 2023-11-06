using Microsoft.AspNetCore.Mvc.Filters;
using NewsSite.UI.Areas.Author.Controllers;

namespace NewsSite.UI.Filters.ActionFilters
{
    public class ArticleRequestModelStateCheckActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // before logic

            if (context.Controller is ArticlesController articlesController)
            {
                if (!articlesController.ModelState.IsValid)
                {
                    var articleRequest = context.ActionArguments["articleRequest"];

                    context.Result = articlesController.View(articleRequest); //short-circuits or skips the subsequent action filters & action method
                }
                else
                {
                    await next();
                }
            }
            else
            {
                await next();
            }

            // after logic
        }
    }
}
