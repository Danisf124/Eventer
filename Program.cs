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
        static UserViewModel user_view_model = new UserViewModel();
        
        static LocationViewModel location_view_model = new LocationViewModel();

        static EventViewModel event_view_model = new EventViewModel();

        static void Main(string[] args)
        {

            Console.WriteLine(" Welcome To Eventer ");

            bool login_or_registered = false;

            while(true)
            {
                // Registration / login cycle
                while(!login_or_registered)
                {
                    Console.WriteLine("[ 0 ] - exit ");
                    Console.WriteLine("[ 1 ] - Registration ");
                    Console.WriteLine("[ 2 ] - Login ");
                    string choice = Console.ReadLine() ?? "";
                    
                    switch (choice)
                    {
                        case "0":
                            Console.WriteLine("Goodbye");
                            user_view_model.LogOut();
                            Environment.Exit(0);
                            break;

                        case "1":

                            login_or_registered = true;

                            if(Registration())
                            {
                                login_or_registered = true;
                                break;
                            }
                        
                            break;

                        case "2":
                            
                            if(Login())
                            {
                                login_or_registered = true;
                                break;
                            }

                            break;

                        default:
                            Console.WriteLine("Invalid option");
                            break;

                    }
                }
                
                while(login_or_registered)
                {
                    Console.WriteLine("-------------------");
                    Console.WriteLine("Choose your option");
                    Console.WriteLine("[ 0 ] - exit ");
                    Console.WriteLine("[ 1 ] - Find Event ");
                    Console.WriteLine("[ 2 ] - Create Event ");
                    Console.WriteLine("[ 3 ] - Logout");
                    
                    string choice = Console.ReadLine() ?? "";

                    switch (choice)
                    {
                        case "0":
                            Console.WriteLine("Goodbye");
                            user_view_model.LogOut();
                            Environment.Exit(0);
                            break;

                        case "1":
                            FindEvent();
                            break;
                        case "2":
                            CreateEvent();
                            break;
                        case "3":
                            login_or_registered = false;
                            break;
                        default:
                            Console.WriteLine("Invalid option");
                            break;
                    }
                    
                }
                
            }
        }
            
        static DateTime GetDateFromUser(string massage)
        {
            while(true)
            {
                Console.WriteLine(massage);
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

        static Guid ChooseLocation(LocationViewModel location_view_model)
        {
            Console.WriteLine("--------- LOCATIONS ---------");

            List<Location> locations = location_view_model.Locations;

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
                var selected_location = locations[choice- 1];
                Console.WriteLine($"You choose: {selected_location.Title}");

                return selected_location.Id;
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
                    var selected_category = categories[choice - 1];

                    Console.WriteLine($"You choose {selected_category}");
                    return selected_category;
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

                    if((choice > 0) && (choice < categories.Length))
                    {
                        var selected_category = categories[choice - 1];
                        Console.WriteLine($"You choose {selected_category}");
                        return selected_category;
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

            Console.Write("Event title (leas than 50 characters) - ");
            string title = Console.ReadLine() ?? "";

            Console.Write("Event description (leas than 500 characters) -  ");
            string description = Console.ReadLine() ?? "";

            Console.WriteLine("Event start time ");
            DateTime start_time = GetDateFromUser("Use this format: 2026-05-20 18:30");

            Console.WriteLine("Event end time ");
            DateTime end_time = GetDateFromUser("Use this format: 2026-05-20 18:30");

            Console.WriteLine("Event Location ");
            Guid location_id = ChooseLocation(location_view_model);

            Console.WriteLine("Event category");
            Event.Category category = ChooseCategory();

            Console.Write("Event price (in UAH) - ");
            float price = GetPriceFromUser();

            string current_email = user_view_model.CurrentUser!.Email;

            event_view_model.CreateEvent(title, description, start_time, end_time, category, price, location_id, current_email);

            

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
            
            var find_event = event_view_model.SearchEvents(name, category);

            Event? selected_event = ChooseEventFromList(find_event);

            if(selected_event != null)
            {
                Console.WriteLine("------- EVENT OPTIONS -------");
                Console.WriteLine($"title - {selected_event.Title}");
                Console.WriteLine($"description - {selected_event.Description}");
                Console.WriteLine($"Location - {selected_event.LocationId}");
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

            Console.Write("Crate password, at least 6 characters - ");
            string password = Console.ReadLine() ?? "";

            bool is_success = user_view_model.RegisterUser(name, sur_name, email, password);

            if(is_success)
            {
                Console.WriteLine(" Registration complete without problems ");
                Console.WriteLine($"DEBAG: Users in list: {user_view_model.Users.Count}");
                return true;
            }
            else
            {
                Console.WriteLine($"Something went wrong, Error: {user_view_model.ErrorMessage}\n");
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

            bool is_success = user_view_model.LoginUser(email, password);

            if(is_success)
            {
                Console.WriteLine(" Login complete without problems ");
                Console.WriteLine($"DEBAG: Users in list: {user_view_model.Users.Count}");
                return true;
            }
            else
            {
                Console.WriteLine($"Something went wrong, Error: {user_view_model.ErrorMessage}\n");
                return false;
            }
            

        }

    }
}