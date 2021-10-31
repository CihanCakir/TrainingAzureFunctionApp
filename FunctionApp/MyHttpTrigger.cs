using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApp
{
    public static class MyHttpTrigger
    {
        [Function("MyHttpTrigger")]
        public static HttpResponseData GetProduct([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "Products")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("MyHttpTrigger");

            var queryDictionary = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(req.Url.Query);
            var result = queryDictionary["id"];


            logger.LogInformation("Ýd Query Parametresi :" + result);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Ýd Query Parametresi: " + result);
            return response;
        }




        [Function("MyHttpTrigger2")]
        public static HttpResponseData GetProductById([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "Products/id/{id}")] HttpRequestData req,
    FunctionContext executionContext, int id)
        {
            var logger = executionContext.GetLogger("MyHttpTrigger2");
            logger.LogInformation("Gelen Id: " + id);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("GetProductById : " + id);

            return response;
        }


        [Function("MyHttpTrigger3")]
        public static async Task<HttpResponseData> GetProductsByBody([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "Products/Body")] HttpRequestData req,
         FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("MyHttpTrigger3");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            Product product = JsonConvert.DeserializeObject<Product>(requestBody);

            logger.LogInformation("Body içeriði: "+JsonConvert.SerializeObject(product));

            
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            await response.WriteStringAsync(JsonConvert.SerializeObject(product));
            return response;
        }

    }
}
