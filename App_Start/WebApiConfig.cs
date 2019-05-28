using My.NETWebAPI.EnumerationConstraint;
using System.Web.Http;
using System.Web.Http.Routing;

namespace My.NETWebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

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
