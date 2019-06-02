using System.Collections.Generic;
using System.Web.Http;

namespace My.NETWebAPI.Controllers
{
    [RoutePrefix("authen")]
    public class AuthenController : ApiController
    {
        // GET: api/Authen
        [Route("")]
        [AllowAnonymous]
        public IEnumerable<string> Get()
        {
            return new string[] { User.Identity.Name, User.Identity.AuthenticationType};
        }

        // GET: api/Authen/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Authen
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Authen/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Authen/5
        public void Delete(int id)
        {
        }
    }
}
