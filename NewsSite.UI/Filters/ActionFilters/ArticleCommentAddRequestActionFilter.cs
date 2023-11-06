using Microsoft.AspNetCore.Mvc.Filters;
using NewsSite.Core.DataTransferObjects.ArticleObjects.CommentObjects;
using NewsSite.Core.Domain.Models.ArticleModels;
using NewsSite.UI.Controllers;

namespace NewsSite.UI.Filters.ActionFilters
{
    public class ArticleCommentAddRequestActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // before logic
            
            await next();

            // after logic


            var controller = context.Controller;
            if (controller is ArticlesController articlesController)
            {
                articlesController.ViewBag.CommentAddRequest = new CommentAddRequest()
                {
                    ArticleId = (context.ActionArguments["id"] as Guid?)!.Value
                };
            }
        }
    }
}
