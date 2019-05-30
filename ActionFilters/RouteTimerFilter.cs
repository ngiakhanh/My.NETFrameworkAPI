using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace My.NETWebAPI.ActionFilters
{
    // TODO: Decide if your filter should allow multiple instances per controller or
    //       per-method; set AllowMultiple to true if so
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class RouteTimerFilterAttribute : ActionFilterAttribute
    {
        public const string Header = "X-API-Timer";
        public const string TimerPropertyName = "RouteTimerFilter_";
        public string TimerName;

        public RouteTimerFilterAttribute(string name = null)
        {
            TimerName = name;
        }

        /// <summary>
        /// Executed BEFORE the controller action method is called
        /// </summary>
        public override async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            // STEP 1:  Any logic you want to do BEFORE the rest of the action filter chain is 
            //          called, and BEFORE the action method itself.
            var name = TimerName;
            if (String.IsNullOrEmpty(name))
            {
                name = actionContext.ActionDescriptor.ActionName;
            }

            actionContext.Request.Properties[TimerPropertyName + name] = Stopwatch.StartNew();

            // STEP 2: Call the rest of the action filter chain
            await base.OnActionExecutingAsync(actionContext, cancellationToken);

            // STEP 3: Any logic you want to do AFTER the other action filters, but BEFORE
            //         the action method itself is called.
        }

        public override async Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            // STEP 1:  Any logic you want to do BEFORE the rest of the action filter chain is 
            //          called, and AFTER the action method itself.
            var name = TimerName;
            if (String.IsNullOrEmpty(name))
            {
                name = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
            }

            var timer = (Stopwatch)actionExecutedContext.Request.Properties[TimerPropertyName + name];
            var time = timer.ElapsedMilliseconds;

            Trace.WriteLine(actionExecutedContext.Request.Method + " "
                + actionExecutedContext.Request.RequestUri + " "
                + actionExecutedContext.ActionContext.ActionDescriptor.ActionName
                + " - Elapsed time for " + name + " was " + time
                );
            // STEP 2: Call the rest of the action filter chain
            await base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);

            // STEP 3: Any logic you want to do AFTER the other action filters, and AFTER
            //         the action method itself is called.
            actionExecutedContext.Response.Headers.Add(Header, time + " msec");
        }
    }
}