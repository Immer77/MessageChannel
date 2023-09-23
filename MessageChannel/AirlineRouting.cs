using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace MessageChannel
{
    /// <summary>
    /// CONTENT BASED ROUTING
    /// </summary>
    internal class AirlineRouting
    {

        /// <summary>
        /// Routing properties that is made dynamic through a dictionary, so it's possible to add more airlinecompanies as the time goes on.
        /// </summary>
        protected MessageQueue inputQueue;
        private readonly Dictionary<string, MessageQueue> airlineQueues = new Dictionary<string, MessageQueue>();

        public AirlineRouting(MessageQueue inputQueue, Dictionary<string,MessageQueue> airlineQueues)
        {
            this.inputQueue = inputQueue;
            this.airlineQueues = airlineQueues;

            inputQueue.ReceiveCompleted += new ReceiveCompletedEventHandler(OnMessage);
            inputQueue.BeginReceive();
        }

        private void OnMessage(object source, ReceiveCompletedEventArgs asyncResult)
        {
            MessageQueue mq = (MessageQueue)source;
            mq.Formatter = new XmlMessageFormatter(new Type[] { typeof(Airplane) });

            Message message = mq.EndReceive(asyncResult.AsyncResult);
            Airplane airplane = (Airplane)message.Body;

            string airlineName = airplane.Company;
            //Checks if there is a match with the airlinename so that if we follow best naming practices of queues it will be succesfull
            if (airlineQueues.ContainsKey(airlineName))
            {
                MessageQueue aLineQueue = airlineQueues[airlineName];
                aLineQueue.Send(airplane);
                Console.WriteLine($"Sent information to: {airplane.Company}");
            }
            else
            {
                Console.WriteLine("No Airlines With that code");
            }

            mq.BeginReceive();
        }

    }
}
