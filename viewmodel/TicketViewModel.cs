using System;
using System.ComponentModel;

namespace Eventer
{
    internal class TicketViewModel
    {
        public List<Ticket> Tickets {get; private set;}

        public string? ErrorMassage {get; private set;}
        public bool IsBusy {get; private set;}

        const int MaxTickets = 100;  

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

        public bool CreateTicket(Guid userId, Guid eventId)
        {
            ErrorMassage = null;

            try
            {
                if(Tickets.Count >= MaxTickets)
                {
                    throw new Exception($"Event list is full, maximum {MaxTickets} events allowed");
                }

                Ticket ticket = new Ticket(userId, eventId);
                AddTicket(ticket);
                return true;
            }
            catch(Exception ex)
            {
                ErrorMassage = ex.Message;
                return false;   
            }
        }
    }
}