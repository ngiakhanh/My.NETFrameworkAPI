using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace My.NETWebAPI.Handlers
{
    public class X_HTTP_Method_Override_Handler : DelegatingHandler
    {
        readonly string[] _methods = { "PUT", "PATCH", "DELETE", "HEAD", "VIEW" };
        protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //Step 1: Global message-level logic that must be executed BEFORE the request is sent
            //        on to the action method
            if (!request.Method.Equals(HttpMethod.Post))
            {
                return await base.SendAsync(request, cancellationToken);
            }
            else
            {
                if (request.Headers.Contains("X-HTTP-Method-Override"))
                {
                    var method = request.Headers.GetValues("X-HTTP-Method-Override").FirstOrDefault().ToUpperInvariant();
                    if (_methods.Contains(method))
                    {
                        request.Method = new HttpMethod(method);
                    }
                    else
                    {
                        var response = new HttpResponseMessage(System.Net.HttpStatusCode.MethodNotAllowed)
                        {
                            Content = new StringContent("Wrong overriden method")
                        };
                        return await Task.FromResult(response);
                    }
                }
            }

            //Step 2: Call the rest of the pipeline, all the way to a response message
            return await base.SendAsync(request, cancellationToken);

        }
    }
}