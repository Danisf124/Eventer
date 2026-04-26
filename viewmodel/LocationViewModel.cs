using System;
using System.Reflection.Metadata;
using System.Runtime.Serialization;

namespace Eventer
{
    internal class LocationViewModel
    {
        public List<Location> Locations {get; private set;}

        public string? ErrorMessage {get; private set;}
        public bool IsBusy {get; private set;}
        public bool IsEmpty => Locations.Count == 0;

        const int MaxLocations = 100;

        public LocationViewModel()
        {
            ErrorMessage = null;
            IsBusy = false;
            Locations = new List<Location>();

            CreateLocation("nobody know where", "wall st. 12"); // seeding for debug
        }
        
        // Add location to list
        public void AddLocation(Location location)
        {
            Locations.Add(location); 
        }

        public bool CreateLocation(string title, string address, string? contact_info = null)
        {
            IsBusy = true;
            ErrorMessage = null;

            try
            {
                //Checking is it's location already exist
                if (Locations.Any(l => l.Address.Equals(address, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new Exception("Location already exist");
                }

                if(Locations.Count >= MaxLocations)
                {
                    throw new Exception($"Event list is full, maximum {MaxLocations} events allowed");
                }


                Location location = new Location(title, address, contact_info = null);

                AddLocation(location);

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