using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace MessageChannel
{
    internal interface ISubject
    {
        void Attach(AirlineCompany airlineCompany);

        void Detach(AirlineCompany airlineCompany);

        void NotifyAirlineCompanies(Message message);

            
    }
}
