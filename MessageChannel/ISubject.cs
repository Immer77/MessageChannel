using System.Messaging;

namespace MessageChannel
{
    internal interface ISubject
    {
        void Attach(AirlineCompany airlineCompany);

        void Detach(AirlineCompany airlineCompany);

        void NotifyAirlineCompanies(Message message);

            
    }
}
