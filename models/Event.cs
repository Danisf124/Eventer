using System;
using System.Diagnostics.Contracts;
using System.Net.WebSockets;
using System.Reflection;

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

        public enum Category
        {
            // some category
            sport,
            cinema
        };

        private double _price;

        public string Title
        {
            get {return _title;}

            set
            {
                if(string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Title can't be empty");
                }

               

                if(value.Length > 100)
                {
                    throw new ArgumentException("Title can't be longer than 100 characters");
                }

                _title = value.Trim();
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

              

                if(value.Length > 1000)
                {
                    throw new ArgumentException("Description can't be longer than 1000 characters");
                }

                _description = value.Trim();

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

        public bool IsActive {get; set;}

        public Category EventCategory {get; set;}

        public Event (string title, string description, DateTime start_time, 
        DateTime end_time, Guid location_id, Category category, double price, bool is_active )
        {
    
            Id = Guid.NewGuid();
            
            Title = title;

            Description = description;

            StartTime = start_time;

            EndTime = end_time;

            LocationId = location_id;

            EventCategory = category;

            Price = price;

            IsActive = is_active;
        }

        public override string ToString()
        {
            return $"{EventCategory}, {Title} - {Price} грн, початок:({StartTime:dd.MM.yyyy HH.mm})";
        }

    }
}