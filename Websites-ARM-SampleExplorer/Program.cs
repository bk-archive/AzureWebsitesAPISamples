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
            var ServerFarmSample = new ARM_serverFarm_Sample();
            ServerFarmSample.client = ARMClient.client;
            //Initialize the Server Farm Samples
            var websiteSample = new ARM_websites_Sample();
            websiteSample.client = ARMClient.client;


            var operation = 0;
            var mainMenu = true;
            
            var serverFarmMenu = true;
            var serverFarmOperation = 0;
            
            var websiteMenu = true;
            var websiteOperation = 0;

            while (mainMenu)
            {
                Console.Clear();
                Console.WriteLine("...::Select Samples::...");
                Console.WriteLine("1) Server Farm Operations");
                Console.WriteLine("2) Website Operations");
                Console.WriteLine("0) Quit");

                if (int.TryParse(Console.ReadLine(), out operation))
                {
                    switch (operation)
                    {
                        case 1:
                            

                            while (serverFarmMenu)
                            {
                                //Server Farm Operations
                                Console.Clear();
                                Console.WriteLine("...::Server Farm Operations::...");

                                Console.WriteLine("1) Create a new ServerFarm");
                                Console.WriteLine("2) Delete an Existing Server Farm");
                                Console.WriteLine("3) List all Server Farms in a Resource Group");
                                Console.WriteLine("4) Get a specific Server Farms in a Resource Group");
                                Console.WriteLine("0) back to previous menu");

                                int.TryParse(Console.ReadLine(), out serverFarmOperation);

                                switch (serverFarmOperation)
                                {
                                    case 1:
                                        ServerFarmSample.createServerFarm(resourceGroup);
                                        Console.ReadLine();
                                        break;
                                    case 2:
                                        ServerFarmSample.deleteServerFarm(resourceGroup);
                                        Console.ReadLine();
                                        break;
                                    case 3:
                                        ServerFarmSample.listServerFarm(resourceGroup);
                                        Console.ReadLine();
                                        break;
                                    case 4:
                                        ServerFarmSample.getServerFarm(resourceGroup);
                                        Console.ReadLine();
                                        break;
                                    default:
                                        serverFarmMenu = false;
                                        Console.Clear();
                                        Console.Write("Operation Not recognized");
                                        break;
                                }
                            }

                            break;
                        case 2:
                            while (websiteMenu)
                            {
                                websiteOperation = -1;
                                //Website Operations
                                Console.Clear();
                                Console.WriteLine("...::Website Operations::...");

                                Console.WriteLine("1) Create a new Website");
                                Console.WriteLine("2) Delete an Existing Website");
                                Console.WriteLine("3) List all Websites in a Resource Group");
                                Console.WriteLine("4) Get a specific Website in a Resource Group");
                                Console.WriteLine("0) back to previous menu");

                                int.TryParse(Console.ReadLine(), out websiteOperation);

                                switch (websiteOperation)
                                {
                                    case 1:
                                        websiteSample.createWebsite(resourceGroup);
                                        Console.ReadLine();
                                        break;
                                    case 2:
                                        websiteSample.deleteSite(resourceGroup);
                                        Console.ReadLine();
                                        break;
                                    case 3:
                                        websiteSample.listSites(resourceGroup);
                                        Console.ReadLine();
                                        break;
                                    case 4:
                                        websiteSample.getSite(resourceGroup);
                                        Console.ReadLine();
                                        break;
                                    default:
                                        websiteMenu = false;
                                        Console.Clear();
                                        Console.Write("Operation Not recognized");
                                        break;
                                }
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
