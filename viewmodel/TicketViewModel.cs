using System;
using System.ComponentModel;

namespace Eventer
{
    internal class TicketViewModel
    {
        public List<Ticket> Tickets {get; private set;}

        public string? ErrorMassage {get; private set;}
        public bool IsBusy {get; private set;}

        public TicketViewModel()
        {
            Tickets = new List<Ticket>();
            IsBusy = false; 
            ErrorMassage = null;
        }

        public void AddTicket(Ticket ticket)
        {
            Tickets.Add(ticket);
        }
    }
}