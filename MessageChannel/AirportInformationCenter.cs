using System.Messaging;
using System.Text.Json;
using System;
using System.Collections.Generic;

namespace MessageChannel
{
    /// <summary>
    /// Airport information center that receieves info from the air traffic control
    /// </summary>
    internal class AirportInformationCenter : ISubject
    {
        public MessageQueue ETAQueue = new MessageQueue(@".\Private$\airplaneeta");
        private List<AirlineCompany> airlineCompanies = new List<AirlineCompany>();
        public AirportInformationCenter() { }

        public void ReceiveInfoFromATC()
        {
            
            
            ETAQueue.Formatter = new XmlMessageFormatter(new Type[] {typeof(Airplane)});

            try
            {
                Message message = ETAQueue.Receive();
                Airplane airplane = (Airplane)message.Body;

                Console.WriteLine($"Receving information Airplane: {airplane}");

                Console.WriteLine("Forwarding message to Airline company");
                //NotifyAirlineCompanies(message);


            }catch(MessageQueueException mq)
            {
                Console.WriteLine(mq.Message);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return;
        }
        
        /// <summary>
        /// Subject methods when using the observer pattern
        /// </summary>
        /// <param name="airplaneCompany"></param>
        public void Attach(AirlineCompany airplaneCompany)
        {
            airlineCompanies.Add(airplaneCompany);
        }

        public void Detach(AirlineCompany airplaneCompany)
        {
            airlineCompanies.Remove(airplaneCompany);
        }

        public void NotifyAirlineCompanies(Message message)
        {
            using (var publishQueue = new MessageQueue("FormatName:MULTICAST=234.1.1.10:5432")) 
            {
                publishQueue.Send(message);
                foreach (var company in airlineCompanies)
                {
                    company.Update();
                }
            }
            

        }
    }
}
