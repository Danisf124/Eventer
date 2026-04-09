using System.Text.RegularExpressions;
using System;
using System.Diagnostics.Tracing;
using System.Reflection.Metadata.Ecma335;
using Microsoft.VisualBasic;

namespace Eventer
{
    internal class User
    {
        //fields
        public Guid Id { get; private set; }
        
        private string _name;
        
        private string _surName;
        
        private string _email;
        
        public User(string name, string sur_name, string email)
        {
            Name = name;
            SurName = sur_name;
            Email = email;
        }
   
        public string PasswordHash {get; private set;} // password

        public string Name
        {
            get { return _name; }

            set
            {
                if(string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Name can't be empty");
                }

                _name = value;
            }
        }

        public string SurName
        {
            get { return _surName; }

            set
            {
                if(string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Surname can't be empty");
                }

                SurName = value;
            }
        }

        public string Email
        {
            get { return _email; }

            set
            {
                if(string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Email can't be empty");
                }

                // Regex check

                string email_pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

                if(Regex.IsMatch(value, email_pattern))
                {
                    throw new ArgumentException("invalid Email format");
                }

                _email = value;
            }
        }

        public DateOnly RegistrationDate {get; private set;}

        public List<Event.Category> Preferences {get; private set;}

        public List<Guid> SavedEvents {get; private set;}

        public override string ToString()
        {
            return $"name - {_name}, surname - {_surName}, email - {_email}";
        }
    }
}