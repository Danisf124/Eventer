using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Eventer
{
    internal class Location
    {
        public Guid Id {get; private set;}

        private string _title = string.Empty;

        private string _address = string.Empty;

        private string? _contactInfo;

        public string Title
        {
            get {return _title;}

            set
            {
                if( string.IsNullOrWhiteSpace(value))
                {
                    throw new AggregateException("title can't be empty");
                }

                value = value.Trim();

                if(value.Length > 100)
                {
                    throw new ArgumentException("Title can't be longer than 100 character");
                }

                _title = value;
            }
        }

        public string Address
        {
            get {return _address;}

            set
            {
                if(string.IsNullOrWhiteSpace(value))
                {
                    throw new AggregateException("address can't be empty");
                }

                value = value.Trim();

                if(value.Length > 30)
                {
                    throw new AggregateException("address can't be longer than 30 characters");
                }

                _address = value;
            }
        }

        public string? ContactInfo 
        {
            get {return _contactInfo;} 

            set
            {
                value = value?.Trim(); // trimming whitespace

                if(value != null && value.Length > 30)
                {
                    throw new ArgumentException("Contact info can't be longer than 30 characters");
                }

                string phone_pattern = @"^\+?\d{10,15}$"; // Regex phone pattern

                if(value != null && !Regex.IsMatch(value, phone_pattern))
                {
                    throw new ArgumentException("Invalid phone format");
                }

                _contactInfo = value;
            }
        }
    
        public Location(string title, string adders, string? contact_info = null )
        {
            Id = Guid.NewGuid();
            Title = title;
            Address = adders;
            ContactInfo = contact_info;
    
        }

        public override string ToString()
        {
            return $"{Title} - {Address}";
        }


    }
}