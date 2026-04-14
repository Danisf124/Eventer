using System;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks.Dataflow;
using Eventer;





namespace Eventer
{
    class Program
    {
        static UserViewModel user_view_model = new UserViewModel();
        
        static void Main(string[] args)
        {

            Console.WriteLine(" Welcome To Eventer ");

            while(true)
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
                        Registration();
                        break;

                    case "2":
                        Login();
                        break;

                    default:
                        Console.WriteLine("Invalid option");
                        break;

                }
            }
            
            
        }

        static void Registration()
        {
            Console.WriteLine("------ REGISTRATION ------");

            Console.WriteLine("Please, give your information");

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
                Console.WriteLine(" Registration compleat without problems ");
                Console.WriteLine($"DEBAG: Users in list: {user_view_model.Users.Count}");
            }
            else
            {
                Console.WriteLine($"Something went wrong, Error: {user_view_model.ErrorMessage}\n");
            }
            
        }

        static void Login()
        {
            Console.WriteLine("------ LOGIN ------");
            Console.WriteLine("Please, give your information");

            Console.Write("Your email - ");
            string email = Console.ReadLine() ?? "";

            Console.Write("Your password - ");
            string password = Console.ReadLine() ?? "";

            bool is_success = user_view_model.LoginUser(email, password);

            if(is_success)
            {
                Console.WriteLine(" Login compleat without problems ");
                Console.WriteLine($"DEBAG: Users in list: {user_view_model.Users.Count}");
            }
            else
            {
                Console.WriteLine($"Something went wrong, Error: {user_view_model.ErrorMessage}\n");
            }
            

        }

    }
}