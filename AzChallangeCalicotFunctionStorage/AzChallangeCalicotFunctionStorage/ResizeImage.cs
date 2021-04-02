using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzChallangeCalicotFunctionStorage
{
    public static class ResizeImage
    {
        [FunctionName("ResizeImage")]
        public static void Run([BlobTrigger("photos/{name}", 
            Connection = "BlobStorageConnectionString")]Stream myBlob, 
            string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }
    }
}
