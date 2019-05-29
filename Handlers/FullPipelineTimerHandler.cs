using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace My.NETWebAPI.Handlers
{
    public class FullPipelineTimerHandler : DelegatingHandler
    {
        const string _header = "X-API-Timer";
        protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //Step 1: Global message-level logic that must be executed BEFORE the request is sent
            //        on to the action method
            var timer = Stopwatch.StartNew();

            //Step 2: Call the rest of the pipeline, all the way to a response message
            var response = await base.SendAsync(request, cancellationToken);

            //Step 3: Any global message-level logic that must be executed AFTER the request has 
            //        executed, before the final HTTP response message
            var elapsed = timer.ElapsedMilliseconds;
            Trace.WriteLine("-- Pipeline+Action time msec: " + elapsed);
            response.Headers.Add(_header, elapsed + " msec");

            //Step 4: Return the final HTTP response
            return response;
        }
    }
}