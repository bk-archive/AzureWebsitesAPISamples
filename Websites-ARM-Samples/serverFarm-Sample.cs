using Microsoft.Azure.Management.WebSites;
using Microsoft.Azure.Management.WebSites.Models;
using Microsoft.WindowsAzure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Websites_ARM_Samples
{
    public class ARM_serverFarm_Sample
    {
        public WebSiteManagementClient client = null;
        private  ServerFarmCreateOrUpdateParameters serverFarmParameters{get;set;}

        public bool createServerFarm(string resourceGroupName)
        {
            serverFarmParameters = new ServerFarmCreateOrUpdateParameters();
            serverFarmParameters.ServerFarm = new ServerFarm();
            serverFarmParameters.ServerFarm.Properties = new ServerFarmProperties();

            Console.WriteLine("...:::Collect Server Farm Parameters:::...");

            Console.WriteLine("Server Farm Name:");
            serverFarmParameters.ServerFarm.Name = Console.ReadLine();

            Console.WriteLine("Location:");
            serverFarmParameters.ServerFarm.Location = GeoRegionNames.WestUS;

            Console.WriteLine("Worker Size [s|m|l]:");
            var size =  Console.ReadLine();
            var configuredSize = ServerFarmWorkerSize.Small;
            if(size.ToLowerInvariant() == "s")
            {
                configuredSize = ServerFarmWorkerSize.Small;
            }
            else if (size.ToLowerInvariant() =="m")
            {
                configuredSize = ServerFarmWorkerSize.Medium;
            }
            else if (size.ToLowerInvariant()=="l")
            {
                configuredSize = ServerFarmWorkerSize.Large;
            }
            serverFarmParameters.ServerFarm.Properties.WorkerSize = configuredSize;
            serverFarmParameters.ServerFarm.Properties.CurrentWorkerSize = configuredSize;

            serverFarmParameters.ServerFarm.Properties.Status = ServerFarmStatus.Ready;

            Console.WriteLine("Server Farm SKU [free|shared|basic|standard]:");
            serverFarmParameters.ServerFarm.Properties.Sku = Console.ReadLine();

            Console.WriteLine("Number of Workers");
            var number = 1;
            int.TryParse(Console.ReadLine(), out number);
            serverFarmParameters.ServerFarm.Properties.NumberOfWorkers = number;
            serverFarmParameters.ServerFarm.Properties.CurrentNumberOfWorkers = number;

            ServerFarmCreateOrUpdateResponse response = client.ServerFarms.CreateOrUpdate(resourceGroupName, serverFarmParameters);

            Console.WriteLine("Request ID \t" + response.RequestId + "\n" + "HTTP Status Code : \t" + response.StatusCode);

            listServerFarm(resourceGroupName);

            return true;
        }

        public bool listServerFarm(string resourceGroupName)
        {

            Console.WriteLine("...:::Web Hosting Plans:::...");
            //List all the server farms in a resource group
            ServerFarmListResponse sflr = new ServerFarmListResponse();
            sflr = client.ServerFarms.List(resourceGroupName);

            sflr.ServerFarms.ToList<ServerFarm>().ForEach(item =>
                Console.WriteLine(JsonConvert.SerializeObject(item, Formatting.Indented))
                );

            return true;
        }

        public bool deleteServerFarm(string resourceGroupName)
        {
            Console.WriteLine("...:::Delete Web Hosting Plan:::...");

            var ServerFarmName = Console.ReadLine();

            var response = client.ServerFarms.Delete(resourceGroupName, ServerFarmName);
            Console.WriteLine("Request ID \t" + response.RequestId + "\n" + "HTTP Status Code : \t" + response.StatusCode);

            listServerFarm(resourceGroupName);

            return true;

        }
    
        public ServerFarm getServerFarm(string resourceGroupName, string ServerFarmName)
        {
            var response = client.ServerFarms.Get(resourceGroupName, ServerFarmName);
            
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.ServerFarm;
            }
            else
            {
                return null;
            }

            
        }

        public void getServerFarm(string resourceGroupName)
        {
            Console.WriteLine("Server Farm Name: ");
            var serverFarmName = Console.ReadLine();

            var serverFarm = getServerFarm(resourceGroupName, serverFarmName);

            if (serverFarm != null)
            {
                Console.WriteLine(JsonConvert.SerializeObject(serverFarm, Formatting.Indented));
            }
            else 
            {
                Console.WriteLine("Error: ServerFarm Not Found");
            }
        }
    
    
    }
}
