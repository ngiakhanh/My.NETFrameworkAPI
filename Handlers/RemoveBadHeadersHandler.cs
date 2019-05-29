using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace My.NETWebAPI.Handlers
{
    public class RemoveBadHeadersHandler : DelegatingHandler
    {
        readonly string[] _badHeaders = { "X-Powered-By", "X-AspNet-Version", "Server" };
        protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //Step 2: Call the rest of the pipeline, all the way to a response message
            var response = await base.SendAsync(request, cancellationToken);

            //Step 3: Any global message-level logic that must be executed AFTER the request has 
            //        executed, before the final HTTP response message
            foreach (var h in _badHeaders)
            {
                response.Headers.Remove(h);
            }

            //Step 4: Return the final HTTP response
            return response;
        }
    }
}