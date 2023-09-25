using System;
using System.Collections.Generic;
using System.Messaging;

namespace MessageChannel
{
    public class AirlineRoutingMessage
    {
        protected MessageQueue inputQueue;
        private readonly Dictionary<string, MessageQueue> airlineQueues = new Dictionary<string, MessageQueue>();

        public AirlineRoutingMessage(MessageQueue inputQueue, Dictionary<string, MessageQueue> airlineQueues)
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
            string airlineName = message.Label;

            
            //Checks if there is a match with the airlinename so that if we follow best naming practices of queues it will be succesfull
            if (airlineQueues.ContainsKey(airlineName))
            {
                MessageQueue aLineQueue = airlineQueues[airlineName];
                aLineQueue.Send(message);
                Console.WriteLine($"Sent information to: {airlineName}");
            }
            else
            {
                Console.WriteLine("No Airlines With that code");
            }

            mq.BeginReceive();
        }
    }
}
