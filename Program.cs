using System;
using System.Data.Common;
using System.Globalization;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks.Dataflow;
using Eventer;


namespace Eventer
{
    class Program
    {
        static UserViewModel userViewModel = new UserViewModel();
        
        static LocationViewModel locationViewModel = new LocationViewModel();

        static EventViewModel eventViewModel = new EventViewModel();

        static void Main(string[] args)
        {

            Console.WriteLine(" Welcome To Eventer ");

            bool loginOrRegistered = false;

            while(true)
            {
                // Registration / login cycle
                while(!loginOrRegistered)
                {
                    Console.WriteLine("[ 0 ] - exit ");
                    Console.WriteLine("[ 1 ] - Registration ");
                    Console.WriteLine("[ 2 ] - Login ");
                    string choice = Console.ReadLine() ?? "";
                    
                    switch (choice)
                    {
                        case "0":
                            Console.WriteLine("Goodbye");
                            userViewModel.LogOut();
                            Environment.Exit(0);
                            break;

                        case "1":

                            if(Registration())
                            {
                                loginOrRegistered = true;
                                break;
                            }
                        
                            break;

                        case "2":
                            
                            if(Login())
                            {
                                loginOrRegistered = true;
                                break;
                            }

                            break;

                        default:
                            Console.WriteLine("Invalid option");
                            break;

                    }
                }
                
                while(loginOrRegistered)
                {
                    Console.WriteLine("-------------------");
                    Console.WriteLine("Choose your option");
                    Console.WriteLine("[ 0 ] - Exit ");
                    Console.WriteLine("[ 1 ] - Find Event ");
                    Console.WriteLine("[ 2 ] - Create Event ");
                    Console.WriteLine("[ 3 ] - Logout");
                    
                    string choice = Console.ReadLine() ?? "";

                    switch (choice)
                    {
                        case "0":
                            Console.WriteLine("Goodbye");
                            userViewModel.LogOut();
                            Environment.Exit(0);
                            break;

                        case "1":
                            FindEvent();
                            break;
                        case "2":
                            CreateEvent();
                            break;
                        case "3":
                            loginOrRegistered = false;
                            break;
                        default:
                            Console.WriteLine("Invalid option");
                            break;
                    }
                    
                }
                
            }
        }


        static void CreateLocation()
        {
            Console.WriteLine("Location title - ");
            string title = Console.ReadLine() ?? "";

            Console.WriteLine("Location address - ");
            string address = Console.ReadLine() ?? "";

            Console.WriteLine("Location contact information: +380********* (you can skip this) - ");
            string? contactInfo = Console.ReadLine();

            if(locationViewModel.CreateLocation(title, address, contactInfo))
            {
                Console.WriteLine("Location created ");
                
            }
            else
            {
                Console.WriteLine($"Error: {locationViewModel.ErrorMessage}");
            }

        }

        static DateTime GetDateFromUser(string message)
        {
            while(true)
            {
                Console.WriteLine(message);
                string input = Console.ReadLine() ?? "";

                if(DateTime.TryParse(input, out DateTime result_date))
                {
                    return result_date;
                }
                else
                {
                    Console.WriteLine("Invalid date and time format");
                }
            }
        }

        static Guid ChooseLocation(LocationViewModel locationViewModel)
        {
            Console.WriteLine("--------- LOCATIONS ---------");

            List<Location> locations = locationViewModel.Locations;

            if(locations.Count == 0)
            {
                Console.WriteLine("There is no locations in list, create new");
                return Guid.Empty;
            }
        
            for(int i = 0; i < locations.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {locations[i]}");
            }

            Console.Write("\nChoose location from list: ");
            string input = Console.ReadLine() ?? "";

            if(int.TryParse(input, out int choice) && (choice > 0) && (choice <= locations.Count))
            {
                var selectedLocation = locations[choice- 1];
                Console.WriteLine($"You choose: {selectedLocation.Title}");

                return selectedLocation.Id;
            }
            else
            {
                Console.WriteLine("Wrong choice");
                return Guid.Empty;
            }

        }

        static Event.Category ChooseCategory()
        {
            Console.WriteLine("--------- CATEGORY --------- ");

            
            var categories = (Event.Category[])Enum.GetValues(typeof(Event.Category));

            int index = 1;
            foreach(Event.Category category in categories)
            {
                Console.WriteLine($"[{index}] {category}");
                index++;
            }

            while(true)
            {
                Console.WriteLine("Choose category number");
                string input  = Console.ReadLine() ?? "";

                if(int.TryParse(input, out int choice) && (choice > 0) && (choice <= categories.Length))
                {
                    var selectedCategory = categories[choice - 1];

                    Console.WriteLine($"You choose {selectedCategory}");
                    return selectedCategory;
                }
                else
                {
                    Console.WriteLine("Invalid choice");
                }
            }
        }

        static Event.Category? FindCategory()
        {
            Console.WriteLine("--------- CATEGORY --------- ");

            
            var categories = (Event.Category[])Enum.GetValues(typeof(Event.Category));

            int index = 1;
            foreach(Event.Category category in categories)
            {
                Console.WriteLine($"[{index}] {category}");
                index++;
            }

            Console.WriteLine("[0] Skip");

            while(true)
            {
                Console.WriteLine("Choose category number( or choose 0 for all category)");
                string input  = Console.ReadLine() ?? "";

                if(int.TryParse(input, out int choice))
                {
                    if(choice == 0)
                    {
                        Console.WriteLine("you choose: skip");
                        return null;
                    }

                    if((choice > 0) && (choice <= categories.Length))
                    {
                        var selectedCategory = categories[choice - 1];
                        Console.WriteLine($"You choose {selectedCategory}");
                        return selectedCategory;
                    }

                }
                else
                {
                    Console.WriteLine("Invalid choice");
                }
            }
        }
        
        static float GetPriceFromUser()
        {
            while(true)
            {
                string input = Console.ReadLine() ?? "";

                input = input.Replace(",", ".");

                if(float.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out float price))
                {
                    return price;
                }
                else
                {
                    Console.WriteLine("Invalid price format ");
                }
            }
        }

        static void CreateEvent()
        {
            Console.WriteLine("------ EVENT CREATION ------");

            Console.WriteLine("Please, enter your information");

            Console.Write("Event title (less than 50 characters) - ");
            string title = Console.ReadLine() ?? "";

            Console.Write("Event description (less than 500 characters) -  ");
            string description = Console.ReadLine() ?? "";

            Console.WriteLine("Event start time ");
            DateTime startTime = GetDateFromUser("Use this format: 2026-05-20 18:30");

            Console.WriteLine("Event end time ");
            DateTime endTime = GetDateFromUser("Use this format: 2026-05-20 18:30");

            Console.WriteLine("Event Location ");

            bool isLocationCreate = false;

            Guid locationId = Guid.Empty;            

            while(!isLocationCreate)
            {
                Console.WriteLine("[ 1 ] - Create location ");
                Console.WriteLine("[ 2 ] - Choose location ");
                string input = Console.ReadLine() ?? "";
                switch(input)
                {
                    case "1":
                        CreateLocation();
                        break;
                    case "2":
                        locationId = ChooseLocation(locationViewModel);
                        isLocationCreate = true;
                        break;
                    default:
                        Console.WriteLine("Wrong input");
                        break;
                }
                
            }

            Console.WriteLine("Event category");
            Event.Category category = ChooseCategory();

            Console.Write("Event price (in UAH) - ");
            float price = GetPriceFromUser();

            string current_email = userViewModel.CurrentUser!.Email;

            eventViewModel.CreateEvent(title, description, startTime, endTime, category, price, locationId, current_email);

        }

        static Event? ChooseEventFromList(List<Event> events)
        {
            if(events.Count == 0)
            {
                Console.WriteLine("There is no Events in list");
                return null;
            }

            for(int i = 0; i < events.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {events[i]}");
            }

            Console.WriteLine("[0] return to menu");

            while(true)
            {
                Console.WriteLine("Choose event from list by number");
                string input = Console.ReadLine() ?? "";

                if(int.TryParse(input, out int choice))
                {
                    if(choice == 0)
                    {
                        return null;
                    }

                    if((choice > 0) && (choice <= events.Count))
                    {
                        return events[choice - 1];
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input");
                }
            }
        }

        static void FindEvent()
        {
            Console.WriteLine("------- EVENT FIND -------");

            Console.Write("Event name - ");
            string name = Console.ReadLine() ?? "";
            
            Console.WriteLine("Event Category ");
            Event.Category? category = FindCategory();
            
            var find_event = eventViewModel.SearchEvents(name, category);

            Event? selectedEvent = ChooseEventFromList(find_event);

            if(selectedEvent != null)
            {
                Console.WriteLine("------- EVENT OPTIONS -------");
                Console.WriteLine($"title - {selectedEvent.Title}");
                Console.WriteLine($"description - {selectedEvent.Description}");
                Console.WriteLine($"Location - {selectedEvent.LocationId}");
            }
            
        }

        static bool Registration()
        {
            Console.WriteLine("------ REGISTRATION ------");

            Console.WriteLine("Please, enter your information");

            Console.Write("Your name - ");
            string name = Console.ReadLine() ?? "";

            Console.Write("Your surname - ");
            string sur_name = Console.ReadLine() ?? "";

            Console.Write("Your email - ");
            string email = Console.ReadLine() ?? "";

            Console.Write("Create password, at least 6 characters - ");
            string password = Console.ReadLine() ?? "";

            bool isSuccess = userViewModel.RegisterUser(name, sur_name, email, password);

            if(isSuccess)
            {
                Console.WriteLine(" Registration complete without problems ");
                Console.WriteLine($"DEBUG: Users in list: {userViewModel.Users.Count}");
                return true;
            }
            else
            {
                Console.WriteLine($"Something went wrong, Error: {userViewModel.ErrorMessage}\n");
                return false;
            }
            
        }

        static bool Login()
        {
            Console.WriteLine("------ LOGIN ------");
            Console.WriteLine("Please, enter your information");

            Console.Write("Your email - ");
            string email = Console.ReadLine() ?? "";

            Console.Write("Your password - ");
            string password = Console.ReadLine() ?? "";

            bool isSuccess = userViewModel.LoginUser(email, password);

            if(isSuccess)
            {
                Console.WriteLine(" Login complete without problems ");
                Console.WriteLine($"DEBAG: Users in list: {userViewModel.Users.Count}");
                return true;
            }
            else
            {
                Console.WriteLine($"Something went wrong, Error: {userViewModel.ErrorMessage}\n");
                return false;
            }
            

        }

    }
}