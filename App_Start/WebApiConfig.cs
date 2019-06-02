using My.NETWebAPI.AuthenticationFilters;
using My.NETWebAPI.EnumerationConstraint;
using My.NETWebAPI.Handlers;
using System.Web.Http;
using System.Web.Http.Routing;

namespace My.NETWebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Disable authentication in host level (before Web API)
            config.SuppressHostPrincipal();

            // Web API configuration and services
            config.Filters.Add(new BasicAuthenticationFilterAttribute());
            config.Filters.Add(new AuthorizeAttribute());

            //config.MessageHandlers.Add(new ApiKeyHeaderHandler());
            //config.MessageHandlers.Add(new FullPipelineTimerHandler());
            //config.MessageHandlers.Add(new RemoveBadHeadersHandler());
            //config.MessageHandlers.Add(new X_HTTP_Method_Override_Handler());
            config.MessageHandlers.Add(new ForwardedHeadersHandler());

            //config.Filters.Add(new RouteTimerFilterAttribute("Global"));

            // Web API routes
            //config.MapHttpAttributeRoutes();
            var constraintResolver = new DefaultInlineConstraintResolver();
            constraintResolver.ConstraintMap.Add("enumcheck", typeof(EnumerationConstraint.EnumerationConstraint));
            constraintResolver.ConstraintMap.Add("validAccount", typeof(EnumerationConstraint.AccountIdConstraint));
            constraintResolver.ConstraintMap.Add("base64", typeof(Base64Constraint));
            config.MapHttpAttributeRoutes(constraintResolver);

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
        }
    }
}
