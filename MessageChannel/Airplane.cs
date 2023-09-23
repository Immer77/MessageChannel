using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageChannel
{
    /// <summary>
    /// Airplane class that is being serialized and sent as information
    /// </summary>
    [Serializable]
    public class Airplane
    {
        public string FlightNr { get; set; }
        public DateTime TimeOfArrival { get; set; }
        
        public string Company { get; set; }
        public Airplane() { }
        public Airplane(string flightNr, DateTime timeOfArrival, string company)
        {
            FlightNr = flightNr;
            TimeOfArrival = timeOfArrival;
            Company = company;
        }

        public override string ToString()
        {
            return $"FlightNr: {FlightNr}, TimeOfArrival: {TimeOfArrival}";
        }
    }
}
