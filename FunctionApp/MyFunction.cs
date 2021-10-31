using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public class MyFunction
    {
        private readonly IService _service;

        public MyFunction(IService service) 
        {
            _service = service;
        }

        [Function("MyFunction")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            FunctionContext executionContext)
        {

            string myApi = Environment.GetEnvironmentVariable("MyApi");

            var logger = executionContext.GetLogger("MyFunction");

            logger.LogInformation("DI Servis Yazýs ý"+_service.Write());


            logger.LogInformation("Enrionmet API VALUE " + myApi);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}
