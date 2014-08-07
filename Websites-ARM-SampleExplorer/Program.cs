using System;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Management.WebSites.Models;

using Websites_ARM_Samples;

namespace Websites_ARM_SampleExplorer
{
    class SampleExplorer
    {
        static void Main(string[] args)
        {
            var settingsReader = ConfigurationManager.AppSettings;


            var aadConfig = new AzureActiveDirectoryConfig();
            
            //Get Azure Active Directory Configuration form App Settings
            try{
                aadConfig.azureSubscriptionID = settingsReader["subscriptionID"];
                aadConfig.aadApplicationName = settingsReader["ActiveDirectoryApplicationName"];
                aadConfig.aadRedirectURL = settingsReader["ActiveDirectoryApplicationRedirect"];
                aadConfig.addClientID = settingsReader["ActiveDirectoryClientID"];
                aadConfig.addTenant = settingsReader["ActiveDirectoryadTenant"];
                aadConfig.aadResourceURI = settingsReader["ActiveDirectoryResourceUri"];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Source);
            }


            //Authenticates ARM websites client
            var ARMClient = new AzureActiveDirectoryHelper(aadConfig);

            //Sets the Resource Group to use for samples
            Console.Write("Resource Group:");
            var resourceGroup = Console.ReadLine();

            //Initialize the Server Farm Samples
            var ServerFarmSample = new serverFarm_ARM_Sample();
            ServerFarmSample.client = ARMClient.client;

            ServerFarmSample.createServerFarm("Personal");

            ServerFarmSample.deleteServerFarm("Personal");

        }
    }
}
