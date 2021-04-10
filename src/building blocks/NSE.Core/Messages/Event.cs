using MediatR;
using System;

namespace NSE.Core.Messages
{
    public class Event : Message, INotification
    {
        public DateTime Timestamp { get; }

        public Event()
        {
            Timestamp = DateTime.Now;
        }

        
    }
}
