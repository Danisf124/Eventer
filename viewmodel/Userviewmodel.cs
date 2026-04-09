using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using BCrypt.Net;


namespace Eventer
{
    internal class UserViewModel
    {
        public List<User> Users {get; private set;}

        public string? ErrorMessage { get; private set; }
        public bool IsBusy { get; private set; }

        public UserViewModel()
        {
            Users = new List<User>();
            ErrorMessage = null;
            IsBusy = false;
        }

        public void AddUser(User new_user)
        {
            Users.Add(new_user);
        }

        public bool RegisterUser(string name, string email, string password)
        {
            IsBusy = true;
            ErrorMessage = null;

            try
            {
                //Checking if email already exist
                if(Users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new Exception(" User email already exist.");
                }

                if(string.IsNullOrWhiteSpace(password) || password.Length < 6)
                {
                    throw new Exception("Password can't be leas than 6 characters");
                }

                string generated_hash = BCrypt.Net.BCrypt.HashPassword(password);

                User user = new User(name, email, password);

                AddUser(user);
                
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