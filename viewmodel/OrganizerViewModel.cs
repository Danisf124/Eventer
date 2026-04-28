using System;

namespace Eventer
{
    internal class OrganizerViewModel : UserViewModel
    {
        public OrganizerViewModel(): base() {}

        public bool UpgradeToOrganizer(string contactInfo)
        {
        
            if (CurrentUser == null || CurrentUser is Organizer) return false;

            Organizer newOrganizer = new Organizer(CurrentUser.Name, CurrentUser.SurName, CurrentUser.Email, contactInfo);
            newOrganizer.PasswordHash = CurrentUser.PasswordHash;
            
            foreach(var eventId in CurrentUser.RegisteredEvents)
            {
                newOrganizer.RegisteredEvents.Add(eventId);
            }
            
            Users.Remove(CurrentUser);
            Users.Add(newOrganizer);

            CurrentUser = newOrganizer;
            return true;
        }
        
        
    }
}