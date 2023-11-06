using Microsoft.AspNetCore.Mvc.Filters;
using NewsSite.Core.Enums.Application;
using NewsSite.Core.Helpers;
using NewsSite.UI.Controllers;

namespace NewsSite.UI.Filters.ActionFilters
{
    public class SortOptionsActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // before logic

            var sortByOptions = SortOptionsHelper.GetSortByOptions();

            // if sortBy argument is not present in request or is not valid, set it to default value (Date)
            if (context.ActionArguments.ContainsKey("sortBy"))
            {
                if (sortByOptions.Any(o => o.Key == (SortAttributes)context.ActionArguments["sortBy"]!) == false)
                {
                    context.ActionArguments["sortBy"] = SortAttributes.Date;
                }             
            }

            await next();

            // after logic

            ArticlesController controller = (ArticlesController)context.Controller;

            var parameters = context.HttpContext.Request.Query;

            // collect parameters from request
            if (parameters != null)
            {
                if (parameters.ContainsKey("sortBy"))
                {
                    if (Enum.TryParse(parameters["sortBy"], out SortAttributes sortAttribute))
                    {
                        controller.ViewBag.CurrentSortByOption = sortAttribute;
                    }
                }
            }

            controller.ViewBag.SortByOptions = sortByOptions;

        }
    }
}
