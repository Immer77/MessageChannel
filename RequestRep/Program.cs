using MessageChannel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace RequestRep
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AirlineCompany SAS = new AirlineCompany("sas");
            AirlineCompany KLM = new AirlineCompany("klm");
            AirlineCompany SW = new AirlineCompany("sw");

            Airplane KL1902 = new Airplane("KL1902", DateTime.Now, KLM.Name);
            Airplane SA9812 = new Airplane("SA9812", DateTime.Now, SAS.Name);
            Airplane SW9911 = new Airplane("SW9911", DateTime.Now, SW.Name);


            AirportInformationCenter AIC = new AirportInformationCenter();
            AirTrafficControl ATC = new AirTrafficControl();
            Monitor monitor = new Monitor();
          
            // Sending 
            ATC.SendToaToAIC(SA9812);
            // Receiving information from AIC
            AIC.ReceiveInfoFromATC();

            SAS.SendToAIC();

            SAS.ReceiveFromAIC();

            Console.ReadLine();

        }
    }
}
