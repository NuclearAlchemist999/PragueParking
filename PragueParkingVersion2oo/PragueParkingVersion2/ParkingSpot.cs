using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;


namespace PragueParkingVersion2
{
    class ParkingSpot
    {
        public int Number { get; set; }
        public int Size { get; set; }
        public int AvailableSize { get; set; }
        public List<Vehicle> Vehicles { get; set; }

        public static string ParkingSpotsPath()
        {
            return @"../../../parkingspots.json";
        }

        public static List<ParkingSpot> ParkingSpotsData()
        {
            string file = File.ReadAllText(ParkingSpotsPath());
            List<ParkingSpot> spots = JsonConvert.DeserializeObject<List<ParkingSpot>>(file);
            return spots;
        }

        public static void ParkingTicket(Vehicle vehicle, int spot)
        {
            Console.Clear();
            Console.WriteLine("---------------------------");
            Console.WriteLine("Vehicle type: {0}\n", vehicle.Type);
            Console.WriteLine("Your reg number: {0}\n", vehicle.RegNumber);
            Console.WriteLine("Spot: {0}\n", spot);
            Console.WriteLine("Time: {0}\n", vehicle.Time);
            Console.WriteLine("---------------------------");
            Console.ReadLine();
            Console.Clear();


        }
        public static void CheckOutInfo(double price, double minutes, string time)
        {
            Console.Clear();
            string parkingText = minutes <= 10 ? "Free parking.\n" : $"Price: {price} CZK\n";
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Parking duration: {0}\n", time);
            Console.WriteLine(parkingText);
            Console.WriteLine("-----------------------------");
            Console.ReadLine();
            Console.Clear();
        }

        public static bool CheckSpotSpace(int spot, int size)
        {
            List<ParkingSpot> spots = ParkingSpotsData();

            if (spots[spot - 1].AvailableSize < size)
            {
                return true;
            }

            return false;
        }

        public static bool CheckIfIsAlreadyParked(string regNum)
        {
            List<ParkingSpot> spots = ParkingSpotsData();

            foreach (ParkingSpot spot in spots)
            {
                Vehicle vehicle = spot.Vehicles.Find(v => v.RegNumber == regNum);
                
                if (vehicle != null)
                {
                    return true;
                }
                
            }
            return false;
        }

        public static bool CheckCharLength(string regNum)
        {
            if (regNum.Length <= 10 && regNum.Length >= 3)
            {
                return true;
            }
            return false; 
        }

        public static void PrintParkingSpots()
        {
            Console.Clear();
            List<ParkingSpot> spots = ParkingSpotsData();

            for (int i = 0; i < spots.Count; i++)
            {
                string print = "";
                
                foreach (Vehicle vehicle in spots[i].Vehicles)
                {
                    int diff = DiffCounter(spots, i);
                    print = TypeTemplate(spots, i, vehicle);

                    string space = "-";

                    for (int j = 0; j < diff; j++)
                    {
                        print = print + space;
                    }
                    
                    print = print + "] ";
                    
                    if ((i + 1) % 3 == 0)
                    {
                        print = print + "\n";
                    }
                }
                string emptySpots = EmptySpots(spots, i);

                ColorTemplate(emptySpots, spots[i].AvailableSize, i);
                ColorTemplate(print, spots[i].AvailableSize, i);
                
                
            }
            Console.ReadLine();
            Console.Clear();
        }

        static string EmptySpots(List<ParkingSpot> spots, int index)
        {
            string empty = "";

            string prefix = index + 1 < 10 ? "000" : index + 1 < 100 ? "00" : index + 1 < 1000 ? "0" : ""; 

            if (spots[index].Vehicles.Count == 0)
            {
                empty = $"[{prefix}{index + 1}. --------Empty spot--------] ";

                if ((index + 1) % 3 == 0)
                {
                    empty = empty + "\n";
                }
            }

            return empty;
        }

        public static int DiffCounter(List<ParkingSpot> spots, int index)
        {
            int diff = 0;

            if (spots[index].Vehicles.Count == 2)
            {
                diff = 20 - (spots[index].Vehicles[0].RegNumber.Length + spots[index].Vehicles[1].RegNumber.Length);
            }
            else
            {
                diff = 21 - spots[index].Vehicles[0].RegNumber.Length;

            }

            return diff;
        }

        public static string TypeTemplate(List<ParkingSpot> spots, int index, Vehicle vehicle)
        {
           string prefix = index + 1 < 10 ? "000" : index + 1 < 100 ? "00" : index + 1 < 1000 ? "0" : "";
           string print = "";
           string type = vehicle.Size == (int)VehicleSize.MC ? "MC:>" : "Car:";

           if (spots[index].Vehicles.Count == 2)
           {
                print = $"[{prefix}{index + 1}. {type}>{spots[index].Vehicles[0].RegNumber}/{spots[index].Vehicles[1].RegNumber}";

           }
           else
           {
                print = $"[{prefix}{index + 1}. {type}>{spots[index].Vehicles[0].RegNumber}";
           }

           return print;
        }

        static void ColorTemplate(string str, int size, int index)
        {
            List<ParkingSpot> spots = ParkingSpotsData();

            if (size == 0)
            {
               Console.ForegroundColor = ConsoleColor.Red;
               Console.Write(str);
               Console.ResetColor();
               return;
            }

            if (size == spots[index].Size)
            {
               Console.ForegroundColor = ConsoleColor.DarkGreen;
               Console.Write(str);
               Console.ResetColor();
               return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(str);
            Console.ResetColor();
            

        }
    }
}



