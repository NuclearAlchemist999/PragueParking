using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace PragueParkingVersion2
{
    class Program
    {
        static void Main(string[] args)
        {
            bool running = true;

            while (running)
            {
                Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
                Console.WriteLine("Welcome!");
                Console.WriteLine();
                Console.WriteLine("(1) to add a car:\n");
                Console.WriteLine("(2) to add a MC:\n");
                Console.WriteLine("(3) to remove a vehicle:\n");
                Console.WriteLine("(4) to move a car:\n");
                Console.WriteLine("(5) to move a MC:\n");
                Console.WriteLine("(6) to search for a vehicle:\n");
                Console.WriteLine("(7) to view parking spots:\n");
                Console.WriteLine("(8) to view prices:\n");
                Console.WriteLine("(9) to change settings:\n");
                Console.WriteLine("(10) to reset settings:\n");
                Console.WriteLine("(11) to end program:\n");

                string choice = Console.ReadLine();
                    
                switch (choice)
                {
                    case "1":
                        bool noCarsAllowed = Utils.NoCarsAllowed();
                        if (noCarsAllowed) break;
                        Vehicle.ParkVehicle(new Car("", DateTime.Now, "Car"));
                        break;
                    case "2":
                        bool noMCsAllowed = Utils.NoMCsAllowed();
                        if (noMCsAllowed) break;
                        Vehicle.ParkVehicle(new MC("", DateTime.Now, "MC"));
                        break;
                    case "3":
                        Vehicle.GetVehicle();
                        break;
                    case "4":
                        Vehicle.MoveVehicle(new Car("", DateTime.Now, "Car"));
                        break;
                    case "5":
                        Vehicle.MoveVehicle(new MC("", DateTime.Now, "MC"));
                        break;
                    case "6":
                        Vehicle.SearchVehicle();
                        break;
                    case "7":
                        ParkingSpot.PrintParkingSpots();
                        break;
                    case "8":
                        Utils.ViewPrice();
                        break;
                    case "9":
                        Utils.ChangeSettings();
                        break; 
                    case "10":
                        Utils.ResetSettings();
                        break;
                    case "11":
                        bool isEndingProgram = Utils.EndProgram();
                        if (isEndingProgram) running = false;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid input!");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                }
            } 
         
        }
    }
}
