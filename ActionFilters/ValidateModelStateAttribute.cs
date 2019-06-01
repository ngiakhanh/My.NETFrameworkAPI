using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace My.NETWebAPI.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple =true)]
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public bool BodyRequired { get; set; }
        public override async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
            }
            else
            {
                foreach (var b in actionContext.ActionDescriptor.ActionBinding.ParameterBindings)
                {
                    //if it is a [FromBody]
                    if (b.WillReadBody)
                    {
                        if (!actionContext.ActionArguments.ContainsKey(b.Descriptor.ParameterName)
                            || actionContext.ActionArguments[b.Descriptor.ParameterName] == null)
                        {
                            actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, b.Descriptor.ParameterName + " is required.");
                        }
                        break;
                    }

                }
            }

            await base.OnActionExecutingAsync(actionContext, cancellationToken);
        }
    }
}