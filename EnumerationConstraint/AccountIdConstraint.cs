using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace My.NETWebAPI.EnumerationConstraint
{
    public class AccountIdConstraint : IHttpRouteConstraint
    {
        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName,
            IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            object value;

            if (values.TryGetValue(parameterName, out value) && value != null)
            {
                var newValue = value as string;
                if (!String.IsNullOrEmpty(newValue) && newValue.StartsWith("1234") && newValue.Length > 5)
                {
                    // see if we can find the string in the enumeration members
                    return true;
                }
            }
            throw new HttpResponseException(new HttpResponseMessage()
            {
                StatusCode = (HttpStatusCode)404,
                Content = new StringContent("Invalid account id")
            });
            //return false;
        }
    }
}