using System;
using System.IO;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Storage;

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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


        [Function("attribute")]
        [TableOutput("Products",Connection = "MyAzureStorage")]
        public MultiOutputFunctionInfo AtttributeExecution([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post",Route ="attribute")] HttpRequestData req,
        FunctionContext executionContext)
        {

            string myApi = Environment.GetEnvironmentVariable("MyApi");

            var logger = executionContext.GetLogger("attribute");

            logger.LogInformation("DI Servis Yazýs ý" + _service.Write());


            logger.LogInformation("Enrionmet API VALUE " + myApi);

            var response = req.CreateResponse(HttpStatusCode.OK);

            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            return new MultiOutputFunctionInfo()
            {
                Object = JsonConvert.DeserializeObject<dynamic>(new StreamReader(req.Body).ReadToEnd()),
                HttpResponse = response
            };
        }
    }

    public class MultiOutputFunctionInfo
    {
        [QueueOutput("queueproduct", Connection = "MyAzureStorage")]
        public dynamic Object { get; set; }

        public HttpResponseData HttpResponse { get; set; }
    }
}
