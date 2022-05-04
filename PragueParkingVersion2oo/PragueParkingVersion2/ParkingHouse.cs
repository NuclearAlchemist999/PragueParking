using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace PragueParkingVersion2
{
    class ParkingHouse
    {
        public List<ParkingSpot> parkingSpots { get; set; }
       
        public static void InitParkingHouse()
        {

            Utils parkingConfig = Utils.ParkingConfig();

            List<ParkingSpot> initialParkingSpots = new List<ParkingSpot>();

            for (int i = 0; i < parkingConfig.NumberOfSpots; i++)
            {
                ParkingSpot pSpot = new ParkingSpot();
                pSpot.Number = i + 1;
                pSpot.Size = parkingConfig.SpotSize;
                pSpot.AvailableSize = parkingConfig.SpotSize;
                pSpot.Vehicles = new List<Vehicle>();
               
                initialParkingSpots.Add(pSpot);
            }

            string parkingData = JsonConvert.SerializeObject(initialParkingSpots, Formatting.Indented);
            File.WriteAllText(ParkingSpot.ParkingSpotsPath(), parkingData);
        }

        public static void SaveAndChange() 
        {
            List<ParkingSpot> spots = ParkingSpot.ParkingSpotsData();

            Utils parkingConfig = Utils.ParkingConfig();

            foreach (ParkingSpot spot in spots)
            {
                if (spot.Vehicles.Count == 0 && parkingConfig.SpotSize <= spot.Size)
                {
                    spot.Size = parkingConfig.SpotSize;
                    spot.AvailableSize = parkingConfig.SpotSize;
                }

            }

            if (parkingConfig.NumberOfSpots < spots.Count)
            {
                for (int i = parkingConfig.NumberOfSpots; i < spots.Count; i++)
                {
                    if (spots[i].Vehicles.Count != 0)
                    {
                        Console.WriteLine("\nCannot remove spots where vehicles are parked!");
                        Console.ReadLine();
                        Console.Clear();
                        return;
                    }

                    if (spots[i].Vehicles.Count == 0)
                    {
                        spots.RemoveAt(i--);
                    }
                }
                        
            }

            int diff = parkingConfig.NumberOfSpots - spots.Count;
            
            if (parkingConfig.NumberOfSpots > spots.Count)
            {
                for (int i = 0; i < diff; i++)
                {
                    spots.Add(new ParkingSpot { 
                    Number = i + 1,
                    Size = parkingConfig.SpotSize, 
                    AvailableSize = parkingConfig.SpotSize, 
                    Vehicles = new List<Vehicle>()});
                }
                
            }

            for (int i = 0; i < spots.Count; i++)
            {
                spots[i].Number = i + 1;
                
            }
            Console.WriteLine("\nThe settings are now changed.");
            Console.ReadLine();
            Console.Clear();

            Utils.SaveToFile(spots);
        }
    }
}

