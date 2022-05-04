using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;


namespace PragueParkingVersion2
{
    enum VehicleSize
    {
        Car = 4,
        MC = 2,
        Bus = 16,
        Bicycle = 1,
    }

    class Vehicle
    {
        public string RegNumber { get; set; }
        public int Size { get; set; }
        public DateTime Time { get; set; }
        public string Type { get; set; }
        

        public Vehicle(string regNumber, DateTime time, string type)
        {
            RegNumber = regNumber;
            Time = time;
            Type = type;
        }
       
        public static void ParkVehicle(Vehicle vehicle)
        {
            Console.Clear();
            bool isFullCar = CheckIfFullCar();
            bool isFullMC = CheckIfFullMC();

            if (vehicle.Size == (int)VehicleSize.Car && isFullCar)
            {
                Console.WriteLine("The parking house is full!");
                Console.ReadLine();
                Console.Clear();
                return;
            }

            if (vehicle.Size == (int)VehicleSize.MC && isFullMC)
            {
                Console.WriteLine("The parking house is full!");
                Console.ReadLine();
                Console.Clear();
                return;
            }

            int foundSpot = 0;
            
            if (vehicle.Size == (int)VehicleSize.Car)
            {
                foundSpot = FindCarSpot();
            }
            if (vehicle.Size == (int)VehicleSize.MC)
            {
                foundSpot = FindMCSpot(); 
            }
            
            Console.WriteLine("Enter reg number:");
            string regNumb = Console.ReadLine();

            vehicle.RegNumber = regNumb;

            bool isAlreadyParked = ParkingSpot.CheckIfIsAlreadyParked(regNumb);
            bool validLength = ParkingSpot.CheckCharLength(regNumb);

            if (isAlreadyParked)
            {
                Console.WriteLine("\nThe reg number already exists!");
                Console.ReadLine();
                Console.Clear();
                return;
            }

            if(!validLength)
            {
                Console.WriteLine("\nInvalid character length!");
                Console.ReadLine();
                Console.Clear();
                return;
            }
            
            InsertVehicle(foundSpot, vehicle);
            
            
        }   
        public static int FindCarSpot()
        {
            List<ParkingSpot> spots = ParkingSpot.ParkingSpotsData();
            
            ParkingSpot spot = spots.First(s => s.AvailableSize >= (int)VehicleSize.Car);
            int foundSpot = spot.Number;
           
            return foundSpot;
        }

        public static int FindMCSpot()
        {
            List<ParkingSpot> spots = ParkingSpot.ParkingSpotsData();

            ParkingSpot spot = spots.First(s => s.AvailableSize >= (int)VehicleSize.MC);
            int foundSpot = spot.Number;

            return foundSpot;
        }


        public static void InsertVehicle(int foundSpot, Vehicle vehicle)
        {
            List<ParkingSpot> spots = ParkingSpot.ParkingSpotsData();

            foreach (ParkingSpot spot in spots)
            {
                if (spot.Number == foundSpot)
                {
                    spot.AvailableSize -= vehicle.Size;
                    spot.Vehicles.Add(vehicle);
                    Utils.SaveToFile(spots);
                 
                }

            }
            ParkingSpot.ParkingTicket(vehicle, foundSpot);
        }

        public static void GetVehicle()
        {
            Console.Clear();
            Console.WriteLine("Enter reg number:");
            string regNumb = Console.ReadLine();

            bool isParked = ParkingSpot.CheckIfIsAlreadyParked(regNumb);


            if(!isParked)
            {
                Console.WriteLine("\nThe vehicle was not found.");
                Console.ReadLine();
                Console.Clear();
                return;
            }

            SubtractTime(regNumb);
            RemoveVehicle(regNumb);
        }
        public static void RemoveVehicle(string regNumb)
        {
            List<ParkingSpot> spots = ParkingSpot.ParkingSpotsData(); 

            foreach (ParkingSpot spot in spots)
            {
                Vehicle vehicle = spot.Vehicles.Find(vehicle => vehicle.RegNumber == regNumb);

                if (vehicle is not null)
                {
                    spot.AvailableSize += vehicle.Size;
                    spot.Vehicles.Remove(vehicle);

                    Utils.SaveToFile(spots);
                    return;
                }
            }
        }

        public static void MoveVehicle(Vehicle vehicle)
        {
            List<ParkingSpot> spots = ParkingSpot.ParkingSpotsData();

            Console.Clear();
            bool isFullCar = CheckIfFullCar();
            bool isFullMC = CheckIfFullMC();

            if (vehicle.Size == (int)VehicleSize.Car && isFullCar)
            {
                Console.WriteLine("The parking house is full!");
                Console.ReadLine();
                Console.Clear();
                return;
            }

            if (vehicle.Size == (int)VehicleSize.MC && isFullMC)
            {
                Console.WriteLine("The parking house is full!");
                Console.ReadLine();
                Console.Clear();
                return;
            }

            Console.WriteLine("Enter reg number:");
            string regNumb = Console.ReadLine();

            bool isParked = ParkingSpot.CheckIfIsAlreadyParked(regNumb);


            if (!isParked)
            {
                Console.WriteLine("\nThe vehicle was not found.");
                Console.ReadLine();
                Console.Clear();
                return;
            }

            bool isNotCar = CheckIfCar(regNumb);
            bool isNotMC = CheckIfMC(regNumb);

            if (vehicle.Size == (int)VehicleSize.Car && isNotCar)
            {
                Console.WriteLine("\nChoose a car!");
                Console.ReadLine();
                Console.Clear();
                return;
            }
            if (vehicle.Size == (int)VehicleSize.MC && isNotMC)
            {
                Console.WriteLine("\nChoose a MC!");
                Console.ReadLine();
                Console.Clear();
                return;
            }

            DateTime saveTime = SaveTime(regNumb);
            Console.WriteLine(saveTime);

            RemoveVehicle(regNumb);

            for (int i = 0; i < spots.Count; i++)
            {
                vehicle.RegNumber = regNumb;
                vehicle.Time = saveTime;

                Console.WriteLine("\nChoose a spot:");
                string input = Console.ReadLine();
                int chosenSpot = 0;
                
                while (!int.TryParse(input, out chosenSpot))
                {
                    Console.WriteLine("\nChoose a spot:");
                    input = Console.ReadLine();
                }
                
                while (chosenSpot > spots.Count || chosenSpot < 1)
                {
                    Console.WriteLine("\nInvalid number!");
                    Console.WriteLine("\nChoose a spot:");
                    input = Console.ReadLine();

                    while (!int.TryParse(input, out chosenSpot))
                    {
                        Console.WriteLine("\nChoose a spot:");
                        input = Console.ReadLine();
                    }
                }

                bool isNotEnoughSpace = ParkingSpot.CheckSpotSpace(chosenSpot, vehicle.Size);

                if (isNotEnoughSpace)
                {
                    Console.WriteLine("\nThere is not enough space for this vehicle!");

                }
                if (!isNotEnoughSpace)
                {
                    InsertVehicle(chosenSpot, vehicle);
                    break;
                }
            }
        }

        public static void SearchVehicle()
        {
            List<ParkingSpot> spots = ParkingSpot.ParkingSpotsData();

            Console.Clear();
            Console.WriteLine("Enter reg number:");
            string regNum = Console.ReadLine();
            bool isThere = ParkingSpot.CheckIfIsAlreadyParked(regNum);

            if (!isThere)
            {
                Console.WriteLine("\nThe vehicle does not exist!");
                Console.ReadLine();
                Console.Clear();
                return;
            }

            foreach (ParkingSpot spot in spots)
            {
                Vehicle vehicle = spot.Vehicles.Find(v => v.RegNumber == regNum);

                if (vehicle is not null)
                {
                    ParkingSpot.ParkingTicket(vehicle, spot.Number);
                    return;
                }
            }

        }

        public static bool CheckIfCar(string regNumb)
        {
            List<ParkingSpot> spots = ParkingSpot.ParkingSpotsData();

            foreach (ParkingSpot spot in spots)
            {
                Vehicle vehicle = spot.Vehicles.Find(vehic => vehic.RegNumber == regNumb);

                if (vehicle is not null)
                {
                    if (vehicle.Size != (int)VehicleSize.Car)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool CheckIfMC(string regNumb)
        {
            List<ParkingSpot> spots = ParkingSpot.ParkingSpotsData();

            foreach (ParkingSpot spot in spots)
            {
                Vehicle vehicle = spot.Vehicles.Find(vehic => vehic.RegNumber == regNumb);

                if (vehicle is not null)
                {
                    if (vehicle.Size != (int)VehicleSize.MC)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool CheckIfFullCar()
        {
            List<ParkingSpot> spots = ParkingSpot.ParkingSpotsData();

            bool isFull = spots.TrueForAll(s => s.AvailableSize < (int)VehicleSize.Car);
            return isFull; 
        }

        public static bool CheckIfFullMC()
        {
            List<ParkingSpot> spots = ParkingSpot.ParkingSpotsData();

            bool isFull = spots.TrueForAll(s => s.AvailableSize < (int)VehicleSize.MC);
            return isFull;
        }

        public static DateTime SaveTime(string regNumb)
        {
            List<ParkingSpot> spots = ParkingSpot.ParkingSpotsData();
            DateTime saveTime = DateTime.Now;

            foreach (ParkingSpot spot in spots)
            {
                Vehicle vehicle = spot.Vehicles.Find(vehicle => vehicle.RegNumber == regNumb);

                if (vehicle is not null)
                {
                    saveTime = vehicle.Time;
                    
                }
            }
            return saveTime;
        }

        public static void SubtractTime(string regNumb)
        {
            List<ParkingSpot> spots = ParkingSpot.ParkingSpotsData();
            Utils parkingConfig = Utils.ParkingConfig();

            foreach (ParkingSpot spot in spots)
            {
                Vehicle vehicle = spot.Vehicles.Find(vehicle => vehicle.RegNumber == regNumb);

                if (vehicle is not null)
                {
                    TimeSpan duration = DateTime.Now.Subtract(vehicle.Time);
                    string formatted = duration.ToString(@"dd\.hh\:mm\:ss");
                    double minutes = (DateTime.Now - vehicle.Time).TotalMinutes;

                    double span = Math.Ceiling((DateTime.Now - vehicle.Time).TotalHours);
                    int vehiclePrice = vehicle.Size == (int)VehicleSize.MC ? parkingConfig.McPrice : parkingConfig.CarPrice;
                    double price = vehiclePrice * span;
                    
                    ParkingSpot.CheckOutInfo(price, minutes, formatted);
                    
                }
            }
        }
    }
}