using My.NETWebAPI.EnumerationConstraint;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace My.NETWebAPI.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum Widgets
        {
            Bolt,
            Screw,
            Nut,
            Motor
        }

        // GET: api/Products/Widget/xxx
        [Route("~/widget/{widget:enumcheck(My.NETWebAPI.Controllers.ProductsController+Widgets)}", Order = 1)]
        [AcceptVerbs("GET")]
        public string GetProductsWithWidget(Widgets widget)
        {
            return "widget-" + widget;
        }

        // GET: api/Products
        [HttpGet, Route("", Order = 2)]
        public IEnumerable<string> GetAllProducts()
        {
            return new string[] { "product1", "product2" };
        }

        // GET: api/Products/5
        [HttpGet, Route("{id:int?:range(1000,3000)}", Name = "GetById")]
        public string GetProduct(int id = 0)
        {
            return "product-" + id;
        }

        // GET: api/Products/5
        [HttpGet, Route("{id:int=0:range(-2000,-1000)}", Name = "GetByIds")]
        public string GetProductId(int id)
        {
            return "product-" + id;
        }

        // GET: api/Products/5/orders/custid
        [HttpGet, Route("{id:int:range(1000,3000)}/orders/{custId}")]
        public string GetProductForCustomer(int id, string custId)
        {
            return "product-orders-" + custId;
        }

        [HttpPost, Route("{productId:int= 0:range(1000,3000)}")]
        public HttpResponseMessage CreateProduct([FromUri] int prodId)
        {
            var response = Request.CreateResponse(HttpStatusCode.Created);

            string uri = Url.Link("GetById", new { id = prodId });
            response.Headers.Location = new System.Uri(uri);
            return response;
        }

        // POST: api/Products
        [HttpPost, Route("")]
        public void CreateProduct([FromBody]string value)
        {
        }

        // PUT: api/Products/5
        [HttpPut, Route("{id:int:range(1000,3000)}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Products/5
        [HttpDelete, Route("{id:int:range(1000,3000)}")]
        public void Delete(int id)
        {
        }

        // example base64 binary data, converts to a sample json object:  
        // ew0KICAibnVtYmVyIjogMTIzLA0KICAib2JqZWN0Ijogew0KICAgICJhIjogImIiLA0KICAgICJjIjogImQiLA0KICAgICJlIjogImYiDQogIH0sDQogICJzdHJpbmciOiAiSGVsbG8gV29ybGQiDQp9
        [HttpGet, Route("binary/{array:base64:maxlength(512)}")]
        public string GetBinaryArray([ModelBinder(typeof(Base64ParameterModelBinder))]byte[] array)
        {
            return System.Text.Encoding.UTF8.GetString(array);

        }

        [HttpGet, Route("complex")]
        public IHttpActionResult ComplexTest([FromUri]ComplexTypeDto obj)
        {
            return Json(obj);
        }

        [HttpPut, Route("bodytest")]
        public string BodyTest([FromBody] string value)
        {
            return value;
        }

        [HttpGet, Route("dates/{*myDate:datetime}")]
        public string GetDate(DateTime myDate)
        {
            return myDate.ToLongDateString();
        }

        [HttpGet, Route("segments/{*array:maxlength(256)}")]
        public string[] GetSegments([ModelBinder(typeof(StringArrayWildcardBinder))] string[] array)
        {
            return array;
        }
    }
}