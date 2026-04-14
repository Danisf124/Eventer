using System;
using System.Reflection.Metadata;

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

        

    }
}