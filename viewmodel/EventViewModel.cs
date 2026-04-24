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

        public bool IsEmpty => Events.Count == 0;

        private UserViewModel userViewModel; // User view model for RegisterUser(ln. 136)

        public EventViewModel(UserViewModel userViewModel)
        {
            this.userViewModel = userViewModel;
            Events = new List<Event>();
            ErrorMessage = null;
            IsBusy = false;
        }

        // Add Event to list
        public void AddEvent(Event new_event)
        {
            Events.Add(new_event);
        }
        
        // Create Event for program
        public bool CreateEvent(string title, string description, DateTime start_time, DateTime end_time, Event.Category category, float price, Guid location_id)
        {
            IsBusy = true;
            ErrorMessage = null;

            Console.WriteLine($"DEBUG: CurrentUser = {userViewModel.CurrentUser?.Email ?? "NULL"}");
            Console.WriteLine($"DEBUG: locationId = {location_id}");

            try
            {
                string ownerEmail = userViewModel.CurrentUser!.Email; // setting owner email 
                Event @event = new Event(title, description, start_time, end_time, location_id, category, price, ownerEmail);
                AddEvent(@event);

                return true;
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
            finally
            {
                IsBusy = false;
            }

        }

        /* Cancel Event in 3 steps:
            1. Searching Event in list, if can't find: set null 
            2. Checking date, if event is leas then an hour, not allow delete
            3. If everything alright, cancel even(with soft delete) 
        */
        public bool CancelEvent(Guid event_id)
        {
            IsBusy = true;
            ErrorMessage = null; 

            try
            {

                if(IsEmpty)
                {
                    throw new Exception("No events in list");
                }

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

                target_event.RegisteredUsers.Remove(userViewModel.CurrentUser!.Id); // Removing Event from list

                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
            finally
            {
                IsBusy = false; // unblocking interface
            }
        }   
        
        // Searching events with: keyword, category, price, start time.
        public List<Event> SearchEvents(string? keyword = null, Event.Category? category = null)
        {
            
            IEnumerable<Event> query = Events;

            if(IsEmpty)
            {
                throw new Exception("No events in list");
            }
            
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

        // Register user on event in program
        public bool RegisterUser(Guid selectedEventId)
        {
            ErrorMessage = null;
            IsBusy = true;

            try
            {
                if(IsEmpty)
                {
                    throw new Exception("No events in list");
                }

                var targetEvent = Events.FirstOrDefault(e => e.Id == selectedEventId); // Search Event in list by id

                if(targetEvent == null)
                {
                    throw new Exception("Event not find");
                }               

                // Check if it's active
                if(!targetEvent.IsActive)
                {
                    throw new Exception("Event isn't active");
                }

                // Check time for registration
                if((targetEvent.StartTime - DateTime.Now).TotalHours < 1)
                {
                    throw new Exception("Event almost started");
                }
                
                // Check if user already registered on this event
                bool isAlreadyRegistered = targetEvent.RegisteredUsers.Contains(userViewModel.CurrentUser!.Id); 

                if(isAlreadyRegistered)
                {
                    throw new Exception("User already registered");
                }

                targetEvent.RegisteredUsers.Add(userViewModel.CurrentUser.Id); // Registering user

                userViewModel.CurrentUser.RegisteredEvents.Add(selectedEventId); // Add user to list of registered users
                return true;

            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
            finally
            {
                IsBusy = false;
            }
        }
      
    }
}