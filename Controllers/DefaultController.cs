using My.NETWebAPI.Handlers;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace My.NETWebAPI.Controllers
{
    [RoutePrefix("default")]
    public class DefaultController : ApiController
    {
        // GET: api/Values
        [Route("")]
        [AcceptVerbs("GET", "VIEW", "HEAD")]
        public IEnumerable<string> Get()
        {
            var getByIdUrl = Url.Link("GetById", new { id = 123 });
            return new string[] {
                getByIdUrl,
                Request.GetSelfReferenceBaseUrl().ToString(),
                Request.RebaseUrlForClient(new Uri(getByIdUrl)).ToString(),
                Request.GetClientIpAddress()
            };
        }

        // GET: api/Values/5
        [HttpGet, Route("{id:int}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Values
        [HttpPost, Route("")]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Values/5
        [HttpPut, Route("{id:int?}")]
        public string Put([FromBody]string value, int id = 0)
        {
            return "Put";
        }

        // DELETE: api/Values/5
        [HttpDelete, Route("{id:int}")]
        public void Delete(int id)
        {
        }
    }
}
