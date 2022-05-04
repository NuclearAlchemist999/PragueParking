using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace PragueParkingVersion2
{
    class MC : Vehicle
    {
        public MC(string regNumber, DateTime time, string type) : base(regNumber, time, type)
        {
            Size = (int)VehicleSize.MC;
        }
    }
}
