using Microsoft.Azure.Management.WebSites;
using Microsoft.Azure.Management.WebSites.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Websites_ARM_Samples
{
    public class ARM_websites_Sample
    {
        public WebSiteManagementClient client = null;
        private WebSiteCreateOrUpdateParameters websiteParameters { get; set; }

        public bool createWebsite (string resourceGroupName)
        {

            websiteParameters = new WebSiteCreateOrUpdateParameters();
            websiteParameters.WebSite = new WebSiteBase();
            websiteParameters.WebSite.Properties = new WebSiteBaseProperties();
            var hostnames = new List<string>();

            var ServerFarmSample = new ARM_WebHostingPlan_Sample();
            ServerFarmSample.client = client;


            Console.WriteLine("...:::Collect Server Farm Parameters:::...");
            Console.WriteLine("Site Name: ");
            websiteParameters.WebSite.Name = Console.ReadLine();
            hostnames.Add(websiteParameters.WebSite.Name + ".azurewebsites.net");
            websiteParameters.WebSite.Properties.HostNames = hostnames;

            Console.WriteLine("Web Hosting Plan: ");
            var serverFarm = ServerFarmSample.getWebHostingPlan(resourceGroupName, Console.ReadLine());
            
            if (serverFarm != null)
            {
                websiteParameters.WebSite.Location = serverFarm.Location;
                websiteParameters.WebSite.Properties.ServerFarm = serverFarm.Name;
            }

            var response = client.WebSites.CreateOrUpdate(resourceGroupName, websiteParameters);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine(JsonConvert.SerializeObject(response.WebSite, Formatting.Indented));
            }
            else {
                Console.WriteLine("ERROR: " + response.StatusCode);
            }

            listSites(resourceGroupName);

            return true;
        }

        public bool listSites(string resourceGroupName, bool details = true)
        {
            Console.WriteLine("...:::Sites:::...");
            

            //List all the Sites in a resource group
            var listResponse = new WebSiteListResponse();
            var parameters = new WebSiteListParameters();

            listResponse = client.WebSites.List(resourceGroupName, null);

            if (details)
            {
                listResponse.WebSites.ToList<WebSite>().ForEach(item =>
                Console.WriteLine(JsonConvert.SerializeObject(item, Formatting.Indented))
                );
            }
            else {
                listResponse.WebSites.ToList<WebSite>().ForEach(item => Console.WriteLine(item.Name) );
            }

            return true;
        }

        public bool deleteSite(string resourceGroupName)
        {
            Console.WriteLine("...:::Delete WebSite:::...");
            Console.Write("Website Name: ");
            var name = Console.ReadLine();
            var deleteParameters = new WebSiteDeleteParameters(false, true, true);

            try
            {
                var response = client.WebSites.Delete(resourceGroupName, name, deleteParameters);
                Console.WriteLine("Request ID \t" + response.RequestId + "\n" + "HTTP Status Code : \t" + response.StatusCode);
                listSites(resourceGroupName,false);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Websiten:  \"" + name + "\"  not found");
                Console.WriteLine(e.InnerException.Message);
                Console.WriteLine(e.InnerException.StackTrace);

                return false;
            }
        }

        public bool getSite(string resourceGroupName)
        {
            Console.Write("Website Name: ");
            var name = Console.ReadLine();

            try
            {
                var response = client.WebSites.Get(resourceGroupName, name, null);
                Console.WriteLine(JsonConvert.SerializeObject(response.WebSite, Formatting.Indented));
                return true;
            }
            catch
            {
                Console.WriteLine("Website :  \"" + name + "\"  not found");

                return false;
            }
        }

        public bool webSitesOperations(string resourceGroupName)
        {
            var webSitesOperations = 0;

            //Website Operations
            Console.Clear();
            Console.WriteLine("...::Website Operations::...");

            Console.WriteLine("1) Create a new Website");
            Console.WriteLine("2) Delete an existing Website");
            Console.WriteLine("3) List all Websites in a Resource Group");
            Console.WriteLine("4) List all Websites in a Resource Group with details");
            Console.WriteLine("5) Get a specific Website in a Resource Group");
            Console.WriteLine("0) back to previous menu");

            int.TryParse(Console.ReadLine(), out webSitesOperations);

            switch (webSitesOperations)
            {
                case 1:
                    createWebsite(resourceGroupName);
                    Console.ReadLine();
                    return true;
                case 2:
                    deleteSite(resourceGroupName);
                    Console.ReadLine();
                    return true;
                case 3:
                    listSites(resourceGroupName, false);
                    Console.ReadLine();
                    return true;
                case 4:
                    listSites(resourceGroupName, true);
                    Console.ReadLine();
                    return true;
                case 5:
                    getSite(resourceGroupName);
                    Console.ReadLine();
                    return true;
                default:
                    return false;

            }
        }
    }
}

