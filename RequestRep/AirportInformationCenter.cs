using System;
using System.Collections.Generic;
using System.Messaging;

namespace MessageChannel
{
    /// <summary>
    /// Airport information center that receieves info from the air traffic control
    /// </summary>
    internal class AirportInformationCenter
    {
        public MessageQueue ETAQueue = new MessageQueue(@".\Private$\airplaneeta");
        public MessageQueue requestQueue = new MessageQueue(@".\Private$\sasrequest");
        private List<Message> messages = new List<Message>();
        public AirportInformationCenter()
        {
            requestQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(Airplane) });
            ETAQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(Airplane) });

            // Added this for always waiting for request from airline company
            requestQueue.ReceiveCompleted += new ReceiveCompletedEventHandler(OnReceiveCompleted);
            requestQueue.BeginReceive();
        }

        
        public void OnReceiveCompleted(object source, ReceiveCompletedEventArgs asyncResult)
        {
            MessageQueue requestQueue = (MessageQueue)source;
            Message requestMessage = requestQueue.EndReceive(asyncResult.AsyncResult);

            
            string label = requestMessage.Label;
           
            switch (label)
            {
                case "sas":
                    Console.WriteLine("Received message from SAS checking flightinformation");
                    foreach (Message message in messages)
                    {
                        if (message.Label.Equals("sas"))
                        {
                            MessageQueue replyQueue = requestMessage.ResponseQueue;
                            replyQueue.Formatter = new XmlMessageFormatter(new Type[] {typeof(Airplane) });
                            message.CorrelationId = requestMessage.Id;
                            
                            replyQueue.Send(message);
                        }
                    }
                    break;
                default: break;
            }

            requestQueue.BeginReceive();
            
        }
        public void ReceiveInfoFromATC()
        {

            try
            {
                Message message = ETAQueue.Receive();
                
                messages.Add(message);
                //Console.WriteLine($"Receving information Airplane: {airplane}");

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
        
        
    }
}
