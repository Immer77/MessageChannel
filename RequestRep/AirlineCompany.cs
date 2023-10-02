using RequestRep;
using System;
using System.Messaging;
using System.Text.RegularExpressions;

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
        private ChannelAdapter adapter;



        public AirlineCompany(string name)
        {
            Name = name;
            requestQueue = new MessageQueue(@".\Private$\sasrequest");
            replyQueue = new MessageQueue(@".\Private$\sasreply");

            requestQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(Airplane) });
            replyQueue.MessageReadPropertyFilter.SetAll();
            ((XmlMessageFormatter)replyQueue.Formatter).TargetTypeNames = new string[] { "System.String,mscorlib" };
            adapter = new ChannelAdapter();

        }

        public void SendToAIC()
        {
            
            Message requestMessage = new Message();
            requestMessage.Body = "Requesting information for SAS";
            requestMessage.Label = "sas";
            requestMessage.ResponseQueue = replyQueue;
            requestMessage.TimeToBeReceived = TimeSpan.FromSeconds(10);
            requestMessage.UseDeadLetterQueue = true;
            requestQueue.Send(requestMessage);

            Console.WriteLine("Sent request");
            Console.WriteLine("\tTime:       {0}", DateTime.Now.ToString("HH:mm:ss.ffffff"));
            Console.WriteLine("\tMessage ID: {0}", requestMessage.Id);           
            Console.WriteLine("\tReply to:   {0}", requestMessage.ResponseQueue.Path);
            Console.WriteLine("\tContents:   {0}", requestMessage.Body.ToString());

        }

        public void ReceiveFromAIC()
        {
            Message replyMessage = replyQueue.Receive();
            //Airplane ap = (Airplane)replyMessage.Body;

            Console.WriteLine("Received reply");
            Console.WriteLine("\tTime:       {0}", DateTime.Now.ToString("HH:mm:ss.ffffff"));
            Console.WriteLine("\tMessage ID: {0}", replyMessage.Id);
            Console.WriteLine("\tCorrel. ID: {0}", replyMessage.CorrelationId);
            Console.WriteLine("\tTime of Arrival {0}", replyMessage.Body.ToString());

            string pattern = @"FlightNr:\s+(?<FlightNr>[A-Z0-9]+),\s+TimeOfArrival:\s+(?<TimeOfArrival>.+)$";

            // Use Regex to match the pattern and extract values.
            Match match = Regex.Match(replyMessage.Body.ToString(), pattern);

            if (match.Success)
            {
                string flightNr = match.Groups["FlightNr"].Value;
                string timeOfArrival = match.Groups["TimeOfArrival"].Value;

                adapter.WriteToExcel(flightNr, timeOfArrival);
            }

        }

        public override string ToString()
        {
            return Name;
        }

    }
}
