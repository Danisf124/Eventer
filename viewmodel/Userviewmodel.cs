using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using BCrypt.Net;


namespace Eventer
{
    internal class UserViewModel
    {
        public List<User> Users {get; private set;}

        public User? CurrentUser {get; private set;}

        public string? ErrorMessage { get; private set; }  // Error manages for exception
        public bool IsBusy { get; private set; } // flag for interface blocking
        public bool IsEmpty => Users.Count == 0;

        public UserViewModel()
        {
            Users = new List<User>();
            ErrorMessage = null;
            IsBusy = false;
        }

        // Add User to list
        public void AddUser(User new_user)
        {
            Users.Add(new_user);
        }
        
        public void SetPassword(string password, User user)
        {
            // Hash generating with BCrypt
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        }

        /*  
            Registration User 
            Search for user in list, if user already exist, transfer user to login.
            return true if operation compleat correct.
        */ 
        public bool RegisterUser(string name, string sur_name, string email, string password)
        {
            IsBusy = true;
            ErrorMessage = null; // clear message

            try
            {
                //Checking if email already exist
                if(Users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new Exception(" User email already exist.");
                }

                // Password Hash:

                // Checking password, if empty or less then 6 character: throw exception
                if(string.IsNullOrWhiteSpace(password) || password.Length < 6)
                {
                    throw new Exception("Password can't be leas than 6 characters");
                }
                
                // Creating and adding User to list
                User user = new User(name, sur_name, email);

                SetPassword(password, user);
              
                AddUser(user);

                CurrentUser = user;
                
                // Operation compleat correct
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
        
        
        public bool LoginUser(string email, string password)
        {
            
            IsBusy = true;
            ErrorMessage = null;

            try
            {

                if(IsEmpty)
                {
                    throw new Exception("No users in list");
                }

                var user = Users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

                if(user == null)
                {
                    throw new Exception("User don't exist");
                }

                // Verify password
                bool is_password_correct = BCrypt.Net.BCrypt.Verify(password,user.PasswordHash);
                
                if(!is_password_correct)
                {
                    throw new Exception("Invalid Password");
                }

                CurrentUser = user;

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

        public void LogOut()
        {
            CurrentUser = null;
        }

    }
}