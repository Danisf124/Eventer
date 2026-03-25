using System;

namespace Eventer
{
    internal class Review
    {
        public Guid Id {get; private set;}

        private Guid _eventId;

        private Guid _userId; 

        private int _rating; // from 1 to 5

        public string? Comment {get; set;}

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
            return $"Event id - {_eventId}, User id - {_userId}, rating - {_rating}, comment - {Comment}, created at - {CreatedAt}";
        }
        
    }
}