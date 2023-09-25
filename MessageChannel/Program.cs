using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace MessageChannel
{
    internal class Program
    {
        static void Main(string[] args)
        {

            AirlineCompany SAS = new AirlineCompany("sas", "Denmark");
            AirlineCompany KLM = new AirlineCompany("klm", "Netherlands");
            AirlineCompany SW = new AirlineCompany("sw", "foobar");

            Airplane KL1902 = new Airplane("KL1902", DateTime.Now, KLM.Name);
            Airplane SA9812 = new Airplane("SA9812", DateTime.Now, SAS.Name);
            Airplane SW9911 = new Airplane("SW9911", DateTime.Now, SW.Name);

            
            AirportInformationCenter AIC = new AirportInformationCenter();
            AirTrafficControl ATC = new AirTrafficControl();
            Dictionary<string, MessageQueue> airlineQueues = new Dictionary<string, MessageQueue>
            {
                { "sas", SAS.AirlineCompanyQueue },          // Existing SAS airline
                { "kl", KLM.AirlineCompanyQueue },          // Existing KLM airline
                { "sw", SW.AirlineCompanyQueue }
            };

            // Content-based routing
            //AirlineRouting router = new AirlineRouting(AIC.ETAQueue, airlineQueues);
            // Message router
            AirlineRoutingMessage router = new AirlineRoutingMessage(AIC.ETAQueue, airlineQueues);

            AIC.Attach(SAS);
            AIC.Attach(KLM);
            AIC.Attach(SW);
            // Sending 
            ATC.SendToaToAIC(SA9812);
            // Receiving information from AIC
            AIC.ReceiveInfoFromATC();
            Console.ReadLine();
            
        }
    }
}
