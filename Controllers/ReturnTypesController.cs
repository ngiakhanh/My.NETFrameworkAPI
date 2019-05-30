using My.NETWebAPI.ActionFilters;
using My.NETWebAPI.EnumerationConstraint;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace My.NETWebAPI.Controllers
{
    [RoutePrefix("returntypes")]
    [ClientCacheControlFilterAttribute(ClientCacheControl.NoCache, 10)]
    public class ReturnTypesController : ApiController
    {
        // GET: api/ReturnTypes
        [HttpGet, Route("void")]
        public void ReturnVoid()
        {

        }

        // GET: api/ReturnTypes/5
        [HttpGet, Route("object")]
        public ComplexTypeDto GetObject()
        {
            var dto = new ComplexTypeDto
            {
                String1 = "String1",
                String2 = "String2",
                Int1 = 1,
                Int2 = 2,
                Date1 = DateTime.UtcNow
            };

            //throw error
            throw new InvalidOperationException("I'm sorry. Failed :D");
            return dto;
        }

        [HttpGet, Route("httpresponse")]
        [ResponseType(typeof(ComplexTypeDto))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ComplexTypeDto))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(HttpError))]
        public HttpResponseMessage GetHttpResponse()
        {
            var dto = new ComplexTypeDto
            {
                String1 = "String1",
                String2 = "String2",
                Int1 = 1,
                Int2 = 2,
                Date1 = DateTime.UtcNow
            };

            var response = Request.CreateResponse(dto);
            response.Headers.Add("X-MyCustomHeader", "MyHeaderValue");
            response.ReasonPhrase = "Good";

            //throw error
            response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid");

            return response;
        }

        // POST: api/ReturnTypes
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ReturnTypes/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ReturnTypes/5
        public void Delete(int id)
        {
        }
    }
}
