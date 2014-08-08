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

            var ServerFarmSample = new ARM_serverFarm_Sample();
            ServerFarmSample.client = client;


            Console.WriteLine("...:::Collect Server Farm Parameters:::...");
            Console.WriteLine("Site Name: ");
            websiteParameters.WebSite.Name = Console.ReadLine();
            hostnames.Add(websiteParameters.WebSite.Name + ".azurewebsites.net");
            websiteParameters.WebSite.Properties.HostNames = hostnames;

            Console.WriteLine("Web Hosting Plan: ");
            var serverFarm = ServerFarmSample.getServerFarm(resourceGroupName, Console.ReadLine());
            
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

        public bool listSites(string resourceGroupName)
        {

            Console.WriteLine("...:::Sites:::...");
            
            //List all the Sites in a resource group
            var listResponse = new WebSiteListResponse();

            var parameters = new WebSiteListParameters();

            

            listResponse = client.WebSites.List(resourceGroupName, null);


            listResponse.WebSites.ToList<WebSite>().ForEach(item =>
                Console.WriteLine(JsonConvert.SerializeObject(item, Formatting.Indented))
                );

            return true;
        }

        public bool deleteSite(string resourceGroupName)
        {
            Console.WriteLine("...:::Delete Web Hosting Plan:::...");

            var WebsiteName = Console.ReadLine();

            var response = client.WebSites.Delete(resourceGroupName, WebsiteName, null);
            Console.WriteLine("Request ID \t" + response.RequestId + "\n" + "HTTP Status Code : \t" + response.StatusCode);

            listSites(resourceGroupName);

            return true;

        }

        public void getSite(string resourceGroupName)
        {
            Console.WriteLine("Website Name: ");

            var WebsiteName = Console.ReadLine();

            var response = client.WebSites.Get(resourceGroupName, WebsiteName, null);
            Console.WriteLine(JsonConvert.SerializeObject(response.WebSite, Formatting.Indented));
        }
    }
}
