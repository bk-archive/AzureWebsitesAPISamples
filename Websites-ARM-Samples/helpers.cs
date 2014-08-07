using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Websites_ARM_Samples
{
    class Regions
    {
        public static Dictionary<string, string> _regions = new Dictionary<string, string>
        {
            {"asiaEast", "East Asia"},
            {"asiaSouthEast", "Southeast Asia"},

            {"brazilSouth", "Brazil South"},
        
            {"europeNorth", "North Europe"},
            {"europeWest", "West Europe"},
        
            {"japanEast", "Japan East"},
            {"japanWest", "Japan West"},
        
            {"usCentral", "Central US"},
            {"usEast", "East US"},
            {"usEast2", "East US 2"}, 
            {"usNorthCentral", "North Central US"},
            {"usWest", "West US"},
            {"usSouthCentral", "South Central US"}
        };

        public static string parseRegion(string region)
        {
            string returnValue;

            if (Regions._regions.TryGetValue(region, out returnValue) )
            {
                return returnValue;
            }
            else
            {
                //Default Value
                return "West US";
            }
        }

    }


    class SKU
    {
        public static Dictionary<string, string> _skus = new Dictionary<string, string>
        {
            {"Free", "free"},
            {"Shared", "shared"},
            {"Basic", "basic"},
            {"Standard", "standard"},
        };


        public static string parseSKU(string sku)
        {
            string returnValue;
            if (SKU._skus.TryGetValue(sku, out returnValue))
            {
                return returnValue;
            }
            else
            {
                //Default Value
                return "free";
            }
        }
    }
}
