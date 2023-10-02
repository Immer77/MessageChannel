using MessageChannel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace RequestRep
{
    internal class Monitor
    {
        private MessageQueue deadQueue = new MessageQueue(@"FormatName:DIRECT=OS:.\SYSTEM$;DEADLETTER ");

        public Monitor() 
        {
            
            deadQueue.ReceiveCompleted += new ReceiveCompletedEventHandler(OnReceiveCompleted);
            deadQueue.BeginReceive();
        }
        public void OnReceiveCompleted(object source, ReceiveCompletedEventArgs asyncResult)
        {
            MessageQueue mq = (MessageQueue)source;
            Message requestMessage = mq.EndReceive(asyncResult.AsyncResult);
            Console.WriteLine("Message in deadletter received");
            Console.WriteLine(requestMessage.Label);
            mq.BeginReceive();
        }
    
    }
}
