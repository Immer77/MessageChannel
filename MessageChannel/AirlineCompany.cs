using System;
using System.Messaging;

namespace MessageChannel
{
    [Serializable]
    public class AirlineCompany : IObserver
    {
        /// <summary>
        /// Class to receieve specific flights related to them
        /// </summary>
        public string Name { get; set; }
        public string Origin { get; set; }
        
        public string Path { get; set; }
        public MessageQueue AirlineCompanyQueue { get; set; }

        // Multicast address to be able to use publish-subscribe
        private string Multicastaddress = "234.1.1.10:5432";
       

        public AirlineCompany() { }

        public AirlineCompany(string name, string origin)
        {
            Name = name;
            Origin = origin;
            Path = @".\Private$\" + name;
            AirlineCompanyQueue = new MessageQueue(Path);
            AirlineCompanyQueue.MulticastAddress = Multicastaddress;
            
        }
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Update method that is called using the observer pattern
        /// </summary>
        public void Update()
        {
           // XML formatter
            AirlineCompanyQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(Airplane) });

            try
            {
                // Message from the airlinecompanyqueue
                Message message = AirlineCompanyQueue.Receive();
                Airplane airplane = (Airplane)message.Body;

                Console.WriteLine($"Received Company Airline information: {airplane}");

            }
            catch (MessageQueueException mq)
            {
                Console.WriteLine(mq.Message);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
