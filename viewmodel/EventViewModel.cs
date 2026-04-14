using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace Eventer
{
    internal class EventViewModel
    {
        public List<Event> Events { get; private set; } 

        public string? ErrorMessage { get; private set; } // Error massage for exception
        public bool IsBusy { get; private set; } // flag for interface blocking

        public EventViewModel()
        {
            Events = new List<Event>();
            ErrorMessage = null;
            IsBusy = false;
        }

        // Add Event to list
        public void AddEvent(Event new_event)
        {
            Events.Add(new_event);
        }

        /* Cancel Event in 3 steps:
            1. Searching Event in list, if can't find: sat null 
            2. Checking date, if event is leas then an hour, not allow delete
            3. If everything alright, cancel even(with soft delete) 
        */
        public void CancelEvent(Guid event_id)
        {
            IsBusy = true;
            ErrorMessage = null; // clearing massages

            try
            {
                // trying find Event in Events list(with Linq), e - element
                var target_event = Events.FirstOrDefault(e => e.Id == event_id);

                if(target_event == null)
                {
                    throw new Exception("Can't find Event in list.");
                }

                // Checking time for cancel event
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
                IsBusy = false; // unblocking interface
            }
        }   
        
        
        public List<Event> SearchEvents(string? keyword = null, Event.Category? category = null)
        {
            
            IEnumerable<Event> query = Events;

            
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(e => e.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            }

            if (category.HasValue)
            {
                query = query.Where(e => e.EventCategory == category.Value);
            }

            
            return query.OrderByDescending(e => e.StartTime).ToList();
        }

    

       
        public List<Event> GetEvents(string keyword = "")
        {
            
            var query = Events.Where(e => e.IsActive);
           
            if (!string.IsNullOrWhiteSpace(keyword))
            {
               
                query = query.Where(e => e.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            }
            
            return query.OrderBy(e => e.StartTime).ToList();
        }
    }
}