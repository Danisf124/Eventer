using System;
using System.Reflection.Metadata;

namespace Eventer
{
    internal class Review
    {
        public Guid Id {get; private set;}

        private Guid _eventId;

        private Guid _userId; 

        private int _rating; // from 1 to 5

        private string? _comment;

        public string? Comment 
        {
            get {return _comment;}

            set
            {
                value = value?.Trim(); // trimming whitespace

                if(value != null && value.Length > 300)
                {
                    throw new ArgumentException("Comment can't be longer than 300 characters");
                }

                _comment = value;
            }
        }

        public Guid EventId
        {
            get {return _eventId;}

            set
            {
                if(value == Guid.Empty)
                {
                    throw new ArgumentException("Event id is empty!");
                }
                _eventId = value;
            }
        }

        public Guid UserId 
        {
            get {return _userId;}

            set
            {
                if(value == Guid.Empty)
                {
                    throw new ArgumentException("User id is empty!");
                }
                _userId = value;
            }
        }

        public int Rating
        {
            get {return _rating;}

            set
            {
                if(value < 0 || value > 5)
                {
                    throw new ArgumentException("Invalid rating!");
                }
                _rating = value;
            }
        }

        public DateTime CreatedAt {get; set;}
        

        public Review(Guid event_id, Guid user_id, int rating, string? comment = null)
        {
            Id = Guid.NewGuid();

            EventId = event_id;

            UserId = user_id;

            Rating = rating;

            Comment = comment;

            CreatedAt = DateTime.Now;
        }

        public override string ToString()
        {
            return $"Event id - {EventId}, User id - {UserId}, rating - {Rating}, comment - {Comment}, created at - {CreatedAt}";
        }
        
    }
}