using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NewsSite.Core.DataTransferObjects.ErrorObjects;

namespace NewsSite.UI.Controllers
{
    public class HomeController : Controller
    {
        [Route("/error")]
        public async Task<IActionResult> Error()
        {
            IExceptionHandlerPathFeature? exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPathFeature != null && exceptionHandlerPathFeature.Error != null)
            {
                var errorObject = new ErrorObject()
                {
                    Message = exceptionHandlerPathFeature.Error.Message
                };

                return View(errorObject);
            }

            return RedirectToAction("Index", "Articles");
        }

        [Route("/error/{code:int}")]
        public async Task<IActionResult> Error(int code)
        {
            var errorObject = new ErrorObject()
            {
                Message = $"Error {code}."
            };

            return View(errorObject);
        }
    }
}
