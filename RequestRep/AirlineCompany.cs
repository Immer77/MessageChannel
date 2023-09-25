using System;
using System.Messaging;

namespace MessageChannel
{
    [Serializable]
    public class AirlineCompany
    {
        /// <summary>
        /// Class to receieve specific flights related to them
        /// </summary>
        public string Name { get; set; }
        private MessageQueue requestQueue;
        private MessageQueue replyQueue;
        

        public AirlineCompany(string name, string origin)
        {
            requestQueue = new MessageQueue(@".\Private$\sasrequest");
            replyQueue = new MessageQueue(@".\Private$\sasreply");

            requestQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(Airplane) });
            replyQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(Airplane) });

        }

        public void SendToAIC()
        {
            Message requestMessage = new Message();
            requestMessage.Body = "Requesting information for SAS";
            requestMessage.Label = "sas";
            requestMessage.ResponseQueue = replyQueue;
            requestQueue.Send(requestMessage);

            Console.WriteLine("Sent request");
            Console.WriteLine("\tTime:       {0}", DateTime.Now.ToString("HH:mm:ss.ffffff"));
            Console.WriteLine("\tMessage ID: {0}", requestMessage.Id);
            Console.WriteLine("\tCorrel. ID: {0}", requestMessage.CorrelationId);
            Console.WriteLine("\tReply to:   {0}", requestMessage.ResponseQueue.Path);
            Console.WriteLine("\tContents:   {0}", requestMessage.Body.ToString());

        }

        public void ReceiveFromAIC()
        {
            Message replyMessage = replyQueue.Receive();
            Airplane ap = (Airplane)replyMessage.Body;
            Console.WriteLine("Received reply");
            Console.WriteLine("\tTime:       {0}", DateTime.Now.ToString("HH:mm:ss.ffffff"));
            Console.WriteLine("\tMessage ID: {0}", replyMessage.Id);
            Console.WriteLine("\tTime of Arrival {0}", ap.TimeOfArrival);
            Console.WriteLine("\tFlightNr {0}", ap.FlightNr);
            
        }
        public override string ToString()
        {
            return Name;
        }

    }
}
