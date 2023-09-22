using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace CodingChallenge.API.Filters
{
    public class LogActionFilter : ActionFilterAttribute
    {
     
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Log("OnActionExecuting", context.RouteData);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Log("OnActionExecuted", context.RouteData);
        }

        private void Log(string methodName, RouteData routeData)
        {
            var controllerName = routeData.Values["controller"];
            var actionName = routeData.Values["action"];
            var message = String.Format("{0} controller:{1} action:{2}", methodName, controllerName, actionName);
            // update to logger services for more convenient
            Debug.WriteLine(message, "Action Filter Log");
        }
    }
}
