using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace My.NETWebAPI.ActionFilters
{
    public enum ClientCacheControl
    {
        Public,  //intermediate devices (proxy)
        Private, //browser-only
        NoCache   //no cache at all
    }

    // TODO: Decide if your filter should allow multiple instances per controller or
    //       per-method; set AllowMultiple to true if so
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class ClientCacheControlFilterAttribute : ActionFilterAttribute
    {
        public ClientCacheControl CacheType;
        public double CacheSeconds;
        public ClientCacheControlFilterAttribute(ClientCacheControl cacheType, double cacheSeconds = 60.0)
        {
            CacheType = cacheType;
            CacheSeconds = cacheSeconds;
            if (CacheType == ClientCacheControl.NoCache)
            {
                CacheSeconds = -1;
            }
        }

        public ClientCacheControlFilterAttribute(double cacheSeconds = 60.0) : this(ClientCacheControl.Private, cacheSeconds)
        {

        }

        public override async Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            // STEP 2: Call the rest of the action filter chain
            await base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);

            //https://www.udemy.com/real-world-aspnet-web-api-services-for-net-framework/learn/lecture/9801078#questions/6938720
            if (actionExecutedContext.Exception != null || actionExecutedContext.Response == null || actionExecutedContext.Response.Content == null)
                return;

            //modern browser
            if (CacheType == ClientCacheControl.NoCache)
            {
                actionExecutedContext.Response.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue
                {
                    NoStore = true
                };
                //for older browsers
                actionExecutedContext.Response.Headers.Pragma.TryParseAdd("no-cache");

                //create a date if none present
                if (!actionExecutedContext.Response.Headers.Date.HasValue)
                {
                    actionExecutedContext.Response.Headers.Date = DateTimeOffset.UtcNow;
                }
                actionExecutedContext.Response.Content.Headers.Expires = actionExecutedContext.Response.Headers.Date;
            }
            else
            {
                actionExecutedContext.Response.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue
                {
                    Public = (CacheType == ClientCacheControl.Public),
                    Private = (CacheType == ClientCacheControl.Private),
                    NoCache = false,
                    MaxAge = TimeSpan.FromSeconds(CacheSeconds)
                };

                //create a date if none present
                if (!actionExecutedContext.Response.Headers.Date.HasValue)
                {
                    actionExecutedContext.Response.Headers.Date = DateTimeOffset.UtcNow;
                }
                actionExecutedContext.Response.Content.Headers.Expires = actionExecutedContext.Response.Headers.Date.Value.AddSeconds(CacheSeconds);
            }
        }
    }
}