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

        public bool CreateLocation(string title, string address, string? contact_info = null)
        {
            IsBusy = true;
            ErrorMessage = null;

            try
            {
                if(string.IsNullOrWhiteSpace(title))
                {
                    throw new Exception("Tile can't be empty");
                }

                if(string.IsNullOrWhiteSpace(address))
                {
                    throw new Exception("address can't be empty");
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