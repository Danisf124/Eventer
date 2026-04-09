using System;
using System.Collections.Generic;
using System.Text;

namespace Eventer
{
    internal class Ticket
    {
        public Guid Id { get; private set;}

        private Guid _eventId;

        private Guid _userId;

        public Guid EventId
        {
            get { return _eventId; }

            set 
            {
                if (value == Guid.Empty)
                {
                    throw new ArgumentException("Event id is empty!");
                }
                _eventId = value;
            }
        }

        public Guid UserId
        {
            get { return _userId; }

            set
            {
                if (value == Guid.Empty)
                {
                    throw new ArgumentException("User id is empty!");
                }
                _userId = value;
            }
        }

        public Ticket(Guid user_id, Guid event_id)
        { 
            Id = Guid.NewGuid();
            UserId = user_id;
            EventId = event_id;
        }

        public override string ToString()
        {
            return $"Ticket id - {Id}, User id - {UserId}, event id - {EventId}";
        }

    }
}
