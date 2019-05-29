using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace My.NETWebAPI.Handlers
{
    public class ApiKeyHeaderHandler : DelegatingHandler
    {
        public const string _apiKeyHeader = "X-API-Key";
        //for Swagger
        public const string _apiQueryString = "api_key";
        protected async override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //Step 1: Global message-level logic that must be executed BEFORE the request is sent
            //        on to the action method
            string apiKey = null;

            //if (request.RequestUri.Segments[1].ToLowerInvariant().StartsWith("swagger"))
            //{
            //    return await base.SendAsync(request, cancellationToken);
            //}

            if (request.Headers.Contains(_apiKeyHeader))
            {
                apiKey = request.Headers.GetValues(_apiKeyHeader).FirstOrDefault();
            }
            else
            {
                var queryString = request.GetQueryNameValuePairs();
                var kvp = queryString.FirstOrDefault(a => a.Key.ToLowerInvariant() == _apiQueryString);
                if (!string.IsNullOrEmpty(kvp.Value))
                {
                    apiKey = kvp.Value;
                }
            }

            //if (string.IsNullOrEmpty(apiKey))
            //{
            //    var response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
            //    {
            //        Content = new StringContent("Missing API Key")
            //    };
            //    return Task.FromResult(response);
            //}
            
            //save the value to Properties
            request.Properties.Add(_apiKeyHeader, apiKey);

            //Compress steps 2, 3, 4 into one line
            return await base.SendAsync(request, cancellationToken);
        }
    }

    public static class HttpRequestMessageApiKeyExtension
    {
        public static string GetApiKey(this HttpRequestMessage request)
        {
            if (request == null)
            {
                return null;
            }

            if (request.Properties.TryGetValue(ApiKeyHeaderHandler._apiKeyHeader, out object apiKey))
            {
                return apiKey.ToString();
            }

            return null;
        }
    }
}