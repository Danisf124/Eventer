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

        public LocationViewModel()
        {
            ErrorMessage = null;
            IsBusy = false;
            Locations = new List<Location>();
        }
        
        public void AddLocation(Location location)
        {
            Locations.Add(location); 
        }

        public void CreateLocation(string title, string adders, string? contact_info = null)
        {
            Location location = new Location(title, adders, null);
            AddLocation(location);
        }

        

    }
}