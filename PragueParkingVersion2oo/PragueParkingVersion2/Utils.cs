using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace PragueParkingVersion2
{

    class Utils
    {
        public int NumberOfSpots { get; set; }
        public int SpotSize { get; set; }
        public string[] VehicleTypes { get; set; }
        public int CarPrice { get; set; }
        public int McPrice { get; set; }
      
        public static void SaveToFile(List<ParkingSpot> spots)
        {
            string jsonOutput = JsonConvert.SerializeObject(spots, Formatting.Indented);
            File.WriteAllText(ParkingSpot.ParkingSpotsPath(), jsonOutput);
        }    
        
        public static Utils ParkingConfig()
        {
            string config = File.ReadAllText(@"../../../parkingConfig.json");
            Utils parkingConfig = JsonConvert.DeserializeObject<Utils>(config);
            return parkingConfig;
        }

        public static bool NoCarsAllowed()
        {
            Utils parkingConfig = ParkingConfig();

            if (!parkingConfig.VehicleTypes.Contains("Car"))
            {
                Console.Clear();
                Console.WriteLine("Cars are not allowed!");
                Console.ReadLine();
                Console.Clear();
                return true;
            }
            return false;
        }

        public static bool NoMCsAllowed()
        {
            Utils parkingConfig = ParkingConfig();

            if (!parkingConfig.VehicleTypes.Contains("MC"))
            {
                Console.Clear();
                Console.WriteLine("MC:s are not allowed!");
                Console.ReadLine();
                Console.Clear();
                return true; 
            }
            return false;
        }

        public static void ChangeSettings()
        {
            Console.Clear();

            Console.WriteLine(@"Are you sure? Type ""yes"" to apply:");

            if (Console.ReadLine() == "yes")
            {
                ParkingHouse.SaveAndChange();
            }
            
            Console.Clear();
        }

        public static void ResetSettings()
        {
            Console.Clear();
            
            Console.WriteLine(@"This will delete all data. Type ""yes"" to reset:");
            
            if (Console.ReadLine() == "yes")
            {
                ParkingHouse.InitParkingHouse();
                Console.WriteLine("The settings were successfully changed!");
                Console.ReadLine();
                Console.Clear();
            }

            Console.Clear();
        }

        public static void ViewPrice()
        {
            Utils parkingConfig = ParkingConfig();

            Console.Clear();

            Console.WriteLine("Price for a car: {0} CZK per hour\n", parkingConfig.CarPrice);
            Console.WriteLine("Price for a MC: {0} CZK per hour\n", parkingConfig.McPrice);

            Console.ReadLine();
            Console.Clear();
        }

        public static bool EndProgram()
        {
            Console.Clear();

            Console.WriteLine(@"Type ""yes"" to end program:");

            if (Console.ReadLine() == "yes")
            {
                return true;
            }

            Console.WriteLine("\nPress enter to continue:");
            Console.ReadLine();
            Console.Clear();

            return false;
        }
    }
}
