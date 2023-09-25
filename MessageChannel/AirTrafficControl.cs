using System.Messaging;


namespace MessageChannel
{
    internal class AirTrafficControl
    {
        public AirTrafficControl() { }

        // Air traffic control to send to the airplaneeta queue
        public void SendToaToAIC(Airplane airplane)
        {
            MessageQueue AirPlaneEta = new MessageQueue(@".\Private$\airplaneeta");
            AirPlaneEta.Send(airplane,"sas");
            return;
        }
    }
}
