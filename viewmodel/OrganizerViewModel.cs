using System;

namespace Eventer
{
    internal class OrganizerViewModel : UserViewModel
    {
        public OrganizerViewModel(): base() {}

        public bool RegisterUser(User user, string contactInfo)
        {
            IsBusy = true;
            ErrorMessage = null; // clear message

            try
            {
                if(Users.Count >= MaxUsers)
                {
                    throw new Exception($"Event list is full, maximum {MaxUsers} events allowed");
                }

                Organizer organizer = new Organizer(user.Name, user.SurName, user.Email, contactInfo);

                organizer.PasswordHash = user.PasswordHash;

                CurrentUser = organizer;

                return true;

            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
            finally
            {
                IsBusy = false;// Unblocking interface
            }

        }
        
        
    }
}