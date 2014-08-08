using Microsoft.Azure.Management.WebSites;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Websites_ARM_Samples
{
    public class AzureActiveDirectoryHelper
    {
        //ARM API Endpoint
        private static Uri APIENDPOINT = new Uri(@"https://management.azure.com/");
        public  WebSiteManagementClient client = null;

        public AzureActiveDirectoryHelper(AzureActiveDirectoryConfig configuration)
        {
            var token = GetAuthorizationHeader(configuration);
            var cred = new TokenCloudCredentials(configuration.azureSubscriptionID, token);
            client = new WebSiteManagementClient(cred, APIENDPOINT);
        }

        private string GetAuthorizationHeader(AzureActiveDirectoryConfig configguration)
        {
            AuthenticationResult result = null;

            var context = new AuthenticationContext("https://login.windows.net/" + configguration.addTenant);

            var thread = new Thread(
                () => { result = context.AcquireToken(resource: configguration.aadResourceURI, clientId: configguration.addClientID, redirectUri: new Uri(configguration.aadRedirectURL), promptBehavior: PromptBehavior.Always); }
                );

            thread.SetApartmentState(ApartmentState.STA);
            thread.Name = "AcquiereTokenThread";
            thread.Start();
            thread.Join();

            return result.CreateAuthorizationHeader().Substring("Bearer ".Length);
        }

    }

    public class AzureActiveDirectoryConfig
    {
        public string aadApplicationName { get; set; }
        public string aadRedirectURL { get; set; }
        public string addClientID { get; set; }
        public string addTenant { get; set; }
        public string aadResourceURI { get; set; }
        public string azureSubscriptionID { get; set; }
    }
}
