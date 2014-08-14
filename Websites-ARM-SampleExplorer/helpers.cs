using Microsoft.Azure.Management.WebSites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Websites_ARM_Samples
{
    class Regions
    {


        public static List<string> _regions = new List<string>
        {
            {"East Asia"},
            {"Southeast Asia"},

            {"Brazil South"},
        
            {"North Europe"},
            {"West Europe"},
        
            {"Japan East"},
            {"Japan West"},
        
            {"Central US"},
            {"East US"},
            {"East US 2"}, 
            {"North Central US"},
            {"West US"},
            {"South Central US"}
        };

        public static string getRegion()
        {
            var i = 0;
            var positionX = Console.CursorLeft;
            var positionY = Console.CursorTop;

            while (true)
            {
                Console.CursorLeft = positionX;
                Console.CursorTop = positionY;
                Console.Write(_regions.ElementAt(i));
                
                var index = Console.ReadKey(true);

                if (index.Key == ConsoleKey.UpArrow || index.Key == ConsoleKey.RightArrow)
                {
                    i++;
                    i = i > _regions.Count()-1 ? i % _regions.Count() : i;
                }
                else if (index.Key == ConsoleKey.DownArrow || index.Key == ConsoleKey.LeftArrow)
                {
                    i--;
                    i = i < 0 ? _regions.Count-1 : i;
                }
                else if (index.Key == ConsoleKey.Enter )
                {
                    Console.Write("\n");
                    return _regions.ElementAt(i);
                }
                else
                {
                    //Other key, ignore it.
                }
                Console.CursorLeft = positionX;
                Console.CursorTop = positionY;
                Console.Write("                              ");
            }
        }

    }
    class SKU
    {
        public static List<string> _skus = new List<string>
        {
            {"free"},
            {"shared"},
            {"basic"},
            {"standard"},
        };


        public static string getSKU()
        {
            var i = 0;
            var positionX = Console.CursorLeft;
            var positionY = Console.CursorTop;

            while (true)
            {
                Console.CursorLeft = positionX;
                Console.CursorTop = positionY;
                Console.Write(_skus.ElementAt(i));

                var index = Console.ReadKey(true);

                if (index.Key == ConsoleKey.UpArrow || index.Key == ConsoleKey.RightArrow)
                {
                    i++;
                    i = i > _skus.Count() - 1 ? i % _skus.Count() : i;
                }
                else if (index.Key == ConsoleKey.DownArrow || index.Key == ConsoleKey.LeftArrow)
                {
                    i--;
                    i = i < 0 ? _skus.Count - 1 : i;
                }
                else if (index.Key == ConsoleKey.Enter)
                {
                    Console.Write("\n");
                    return _skus.ElementAt(i).ToLowerInvariant();
                }
                else
                {
                    //Other key, ignore it.
                }
                Console.CursorLeft = positionX;
                Console.CursorTop = positionY;
                Console.Write("                              ");
            }
        }

        public static bool isDeidicated( string sku)
        {
            if (_skus.BinarySearch(sku) >= 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    class SIZE
    {
        private static List<string> _sizes = new List<string>
        {
            {"Small"},
            {"Medium"},
            {"Large"},
        };


        public static ServerFarmWorkerSize getSize()
        {
            var i = 0;
            var positionX = Console.CursorLeft;
            var positionY = Console.CursorTop;

            while (true)
            {
                Console.CursorLeft = positionX;
                Console.CursorTop = positionY;
                Console.Write(_sizes.ElementAt(i));

                var index = Console.ReadKey(true);

                if (index.Key == ConsoleKey.UpArrow || index.Key == ConsoleKey.RightArrow)
                {
                    i++;
                    i = i > _sizes.Count() - 1 ? i % _sizes.Count() : i;
                }
                else if (index.Key == ConsoleKey.DownArrow || index.Key == ConsoleKey.LeftArrow)
                {
                    i--;
                    i = i < 0 ? _sizes.Count - 1 : i;
                }
                else if (index.Key == ConsoleKey.Enter)
                {
                    Console.Write("\n");
                    switch (i)
                    {
                        case 1:
                            return ServerFarmWorkerSize.Medium;
                        case 2:
                            return ServerFarmWorkerSize.Large;
                        case 0:
                            return ServerFarmWorkerSize.Small;
                    }
                }
                else
                {
                    //Other key, ignore it.
                }
                Console.CursorLeft = positionX;
                Console.CursorTop = positionY;
                Console.Write("                              ");
            }
        }
    }
}
