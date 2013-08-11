namespace Peruka.Web.Api
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Modules.HelloWorld;

    public class HelloWorldController : ApiController
    {
        private readonly IHelloService _helloService;

        public HelloWorldController(IHelloService helloService)
        {
            _helloService = helloService;
        }

        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _helloService.Say());
        }
    }
}