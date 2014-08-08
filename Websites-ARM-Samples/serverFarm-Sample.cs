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
    public class ARM_WebHostingPlan_Sample
    {
        public WebSiteManagementClient client = null;
        private  ServerFarmCreateOrUpdateParameters serverFarmParameters{get;set;}

        public bool createWebHostingPlan(string resourceGroupName)
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

            listWebHostingPlan(resourceGroupName);

            return true;
        }

        public bool listWebHostingPlan(string resourceGroupName)
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

        public bool deleteWebHostingPlan(string resourceGroupName)
        {
            Console.WriteLine("...:::Delete Web Hosting Plan:::...");
            Console.Write("Web Hosting Plan Name: ");
            var name = Console.ReadLine();

            try
            {
                var response = client.ServerFarms.Delete(resourceGroupName, name);
                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine("Request ID \t" + response.RequestId + "\n" + "HTTP Status Code : \t" + response.StatusCode);
                }
            }
            catch 
            {
                Console.WriteLine("Web Hosting Plan:  \"" + name + "\"  not found");
            }
            

            

            listWebHostingPlan(resourceGroupName);

            return true;

        }
    
        public ServerFarm getWebHostingPlan(string resourceGroupName, string ServerFarmName)
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

        public void getWebHostingPlan(string resourceGroupName)
        {
            Console.WriteLine("Web Hosting Plan: ");
            var whpName = Console.ReadLine();

            var whp = getWebHostingPlan(resourceGroupName, whpName);

            if (whp != null)
            {
                Console.WriteLine(JsonConvert.SerializeObject(whp, Formatting.Indented));
            }
            else 
            {
                Console.WriteLine("Error: WebHostingPlan \"" + whpName +"\" Not Found");
            }
        }


        public bool webHostingPlanOperations(string resourceGroupName)
        {
            var webHostingPlanOperation = 0;

            //Web Hosting Plan Operations
            Console.Clear();
            Console.WriteLine("...::Web Hosting Plan Operations::...");

            Console.WriteLine("1) Create a new Web Hosting Plan");
            Console.WriteLine("2) Delete an Existing Web Hosting Plan");
            Console.WriteLine("3) List all Web Hosting Plans in a Resource Group");
            Console.WriteLine("4) Get a specific Web Hosting Plan by Name in a Resource Group");
            Console.WriteLine("0) back to previous menu");

            int.TryParse(Console.ReadLine(), out webHostingPlanOperation);

            switch (webHostingPlanOperation)
            {
                case 1:
                    createWebHostingPlan(resourceGroupName);
                    Console.ReadLine();
                    return true;
                case 2:
                    deleteWebHostingPlan(resourceGroupName);
                    Console.ReadLine();
                    return true;
                case 3:
                    listWebHostingPlan(resourceGroupName);
                    Console.ReadLine();
                    return true;
                case 4:
                    getWebHostingPlan(resourceGroupName);
                    Console.ReadLine();
                    return true;
                default:
                    return false;
                    
            }
        }

    }
}
