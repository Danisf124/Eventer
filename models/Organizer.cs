using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Eventer
{
    internal class Organizer : User
    {
        
        private List<double> _rating;

        public List<Review> Reviews {get; private set;}

        private string _contactInfo = string.Empty;

        public string ContactInfo 
        {
            get {return _contactInfo;} 

            set
            {
                value = value.Trim(); // trimming whitespace

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

        public double Rating
        {
            set
            {
                if(!((value * 2) % 1 == 0))
                {
                   throw new ArgumentException("The rating must be a multiple of five.");
                }

                if(value < 1 || value > 5)
                {
                    throw new ArgumentException("Invalid rating use: 1 - 5 ");
                }

                _rating.Add(value);
            }
        }

        public Organizer(string name, string surName, string email, string contactInfo ) : base(name, surName, email)
        {
            ContactInfo = contactInfo;
        }

        public override string ToString()
        {
            return $"name - {Name}, surname - {SurName}, email - {Email}";
        }

    }
}