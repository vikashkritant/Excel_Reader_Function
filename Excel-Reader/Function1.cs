using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Excel_Reader
{
    public class Function1
    {
        [FunctionName("Function1")]
        public void Run([BlobTrigger("exceldata/{name}", Connection = "StorageConnection")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            try
            {
                var ConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
                
                CloudBlobClient serviceClient = storageAccount.CreateCloudBlobClient();
                
                CloudBlobContainer container = serviceClient.GetContainerReference("exceldata");
                
                CloudBlockBlob blob = container.GetBlockBlobReference(name);
                

                using (var memoryStream = new MemoryStream())
                {
                    blob.DownloadToStreamAsync(memoryStream).GetAwaiter().GetResult();
                    memoryStream.Position = 0;
                    using (var reader = new StreamReader(memoryStream))
                    using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
                    {
                        var records = csv.GetRecords<Employee>();
                        foreach (Employee item in records)
                        {
                            Console.WriteLine(item.Name);
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }

        }
    }
}
