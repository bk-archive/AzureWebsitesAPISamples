using System;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Management.WebSites.Models;

using Websites_ARM_Samples;
using System.Threading;

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
            var webHostingPlanSample = new ARM_WebHostingPlan_Sample();
            webHostingPlanSample.client = ARMClient.client;
            //Initialize the Server Farm Samples
            var websiteSample = new ARM_websites_Sample();
            websiteSample.client = ARMClient.client;


            var operation = 0;
            var mainMenu = true;
            
            var webHostingPlanmMenu = true;
            var websiteMenu = true;

            while (mainMenu)
            {
                Console.Clear();
                Console.WriteLine("...::Select Samples::...");
                Console.WriteLine("1) Web Hosting Plan Operations");
                Console.WriteLine("2) Website Operations");
                Console.WriteLine("0) Quit");

                if (int.TryParse(Console.ReadLine(), out operation))
                {
                    switch (operation)
                    {
                        case 1:


                            while (webHostingPlanmMenu)
                            {
                                webHostingPlanmMenu = webHostingPlanSample.webHostingPlanOperations(resourceGroup);
                            }

                            break;
                        case 2:
                            while (websiteMenu)
                            {
                                websiteMenu = websiteSample.webSitesOperations(resourceGroup);
                            }
                            
                            break;
                        case 0:
                            mainMenu = false;
                            Console.Clear();
                            Console.Write("..::GOODBYE::..");
                            Thread.Sleep(2000);
                            break;
                        default:
                            Console.Clear();
                            Console.Write("Operation Not recognized:");
                            break;
                    }
                }
                else {
                    Console.Clear();
                    Console.Write("Operation Not recognized:");
                }
            }
        }
    }
}
