using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;

namespace Eventer
{
    internal class EventViewModel
    {
        public List<Event> Events { get; private set; }

        public string? ErrorMessage { get; private set; }
        public bool IsBusy { get; private set; }

        public EventViewModel()
        {
            Events = new List<Event>();
            ErrorMessage = null;
            IsBusy = false;
        }

        public void AddEvent(Event new_event)
        {
            Events.Add(new_event);
        }

        public void CancelEvent(Guid event_id)
        {
            IsBusy = true;
            ErrorMessage = null;

            try
            {
                // trying find Event in Events list, e - element
                var target_event = Events.FirstOrDefault(e => e.Id == event_id);

                if(target_event == null)
                {
                    throw new Exception("Can't find Event in list.");
                }

                // Checking time for delete event
                if((target_event.StartTime - DateTime.Now).TotalHours < 1)
                {
                    throw new Exception("Cannot cancel event! Less than 1 hour left until start.");
                }
                
                // Soft delete
                target_event.IsActive = false;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsBusy = false;
            }
        }   
        
    }
}