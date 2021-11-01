using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApp
{
    public static class QueueFunction1
    {
        [Function("QueueFunction1")]
        public static void Run([QueueTrigger("queueproduct", Connection = "MyAzureStorage")] dynamic myQueueItem,
            FunctionContext context)
        {
            var logger = context.GetLogger("QueueFunction1");
            logger.LogInformation($"C# Queue trigger function processed: {JsonConvert.SerializeObject(myQueueItem)}");
        }
    }
}
