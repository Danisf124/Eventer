using System;

namespace Eventer
{
    internal class Location
    {
        public Guid Id {get; private set;}

        private string _title;

        private string _adders;

        public string Title
        {
            get {return _title;}

            set
            {
                if( string.IsNullOrWhiteSpace(value))
                {
                    throw new AggregateException("title can't be empty");
                }

                _title = value;
            }
        }

        public string Adders
        {
            get {return _adders;}

            set
            {
                if( string.IsNullOrWhiteSpace(value))
                {
                    throw new AggregateException("adders can't be empty");
                }

                _adders = value;
            }
        }

        public string? ContactInfo {get; set;}
    
        public Location(string name, string adders, string? contact_info = null )
        {
            Id = Guid.NewGuid();
            Title = name;
            Adders = adders;
            ContactInfo = contact_info;
    
        }

        public override string ToString()
        {
            return $"title - {_title}, adders - {_adders}";
        }


    }
}