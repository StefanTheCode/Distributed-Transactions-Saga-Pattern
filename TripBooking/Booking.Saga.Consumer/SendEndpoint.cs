using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Saga.Consumer
{
    public class SendEndpoint
    {
        public static ISendEndpoint Endpoint { get; set; }
    }
}
