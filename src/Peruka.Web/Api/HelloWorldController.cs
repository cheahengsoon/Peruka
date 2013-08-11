namespace Peruka.Web.Api
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    public class HelloWorldController : ApiController
    {
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Hello world!");
        }
    }
}