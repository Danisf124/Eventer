using System;
using System.Diagnostics.Contracts;
using System.Net.WebSockets;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Eventer
{
    internal class Event
    {
        public Guid Id {get; private set;}

        private string _title = string.Empty;

        private string _description = string.Empty;

        private DateTime _startTime;

        private DateTime _endTime;

        private Guid _locationId;

        private string _ownerEmail = string.Empty;

        private double _price;

        public enum Category
        {
            // some category
            sport,
            cinema
        };

        public string Title
        {
            get {return _title;}

            set
            {
                if(string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Title can't be empty");
                }

                value = value.Trim(); // trimming whitespace

                if(value.Length > 50)
                {
                    throw new ArgumentException("Title can't be longer than 50 characters");
                }

                _title = value;
            }
        }



        public string Description
        {
            get {return _description;}

            set
            {
                if(string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("description can't be empty");
                }

                value = value.Trim(); // trimming whitespace

                if(value.Length > 500)
                {
                    throw new ArgumentException("Description can't be longer than 500 characters");
                }

                _description = value;

            }
        }

        public DateTime StartTime
        {
            get {return _startTime;}

            set
            {
                if(value < DateTime.Now)
                {
                    throw new ArgumentException("start time can't be in past");
                }

                _startTime = value;
            }
        }

        public DateTime EndTime
        {
            get {return _endTime;}

            set
            {
                if(value < _startTime)
                {
                    throw new ArgumentException("End time can't be behind start");
                }
                _endTime = value;
            }
        }

        public Guid LocationId
        {
            get {return _locationId;}

            set
            {
                if(value == Guid.Empty)
                {
                    throw new ArgumentException("Location can't be empty!");
                }

                _locationId = value;
            }
        }

        public double Price
        {
            get {return _price;}

            set
            {
                if(value < 0)
                {
                    throw new ArgumentException("Invalid price");
                }

                _price = value;
            }
        }

        public string OwnerEmail
        {
            get {return _ownerEmail;}

            set
            {
                if(string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Owner email can't be empty");
                }

                // Regex check

                value = value.Trim();

                if(value.Length > 30)
                {
                    throw new ArgumentException("Email can't be longer than 30 characters");
                }
                
                string email_pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"; // Regex email pattern

                if(!Regex.IsMatch(value, email_pattern))
                {
                    throw new ArgumentException("Invalid Email format");
                }
            }
        }

        public List<Guid> RegisteredUsers {get; private set;}

        public bool IsActive {get; set;}

        public Category EventCategory {get; set;}

        public Event (string title, string description, DateTime start_time, 
        DateTime end_time, Guid location_id, Category category, double price, string owner_email)
        {
    
            Id = Guid.NewGuid();
            
            Title = title;

            Description = description;

            StartTime = start_time;

            EndTime = end_time;

            LocationId = location_id;

            EventCategory = category;

            Price = price;

            IsActive = true;

            OwnerEmail = owner_email;

            RegisteredUsers = new List<Guid>();
        }

        public override string ToString()
        {
            string formattedDate = StartTime.ToString("yyyy-MM-dd HH:mm");

            return $"{EventCategory}, {Title} - {Price:N2} ₴ , start:({StartTime:dd.MM.yyyy HH.mm})";
        }

    }
}