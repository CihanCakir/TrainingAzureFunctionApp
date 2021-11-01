using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Storage.Queue;

namespace FunctionAppVold
{
    public static class MyHttpTrigger
    {
        [FunctionName("MyHttpTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log, 
            [Table("Products", Connection = "MyAzureStorage")] CloudTable cloudTable,
            [Queue("queueproduct",Connection = "MyAzureStorage")] CloudQueue cloudQueue)
        {


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            Product product = JsonConvert.DeserializeObject<Product>(requestBody);

            TableOperation tableOperation = TableOperation.Insert(product);
            await cloudTable.ExecuteAsync(tableOperation);

            var productString = JsonConvert.SerializeObject(product);

            CloudQueueMessage cloudQueueMessage = new CloudQueueMessage(productString);

            await cloudQueue.AddMessageAsync(cloudQueueMessage);

            return new OkObjectResult(product);
        }
    }
}
