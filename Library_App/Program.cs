using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Security.Cryptography;

namespace Library_App
{
    class Program
    {
        static void Main()
        {
            Library library = new Library("library.json");
            UserAuthManager userManager = new UserAuthManager("users.json");
            userManager.LoadUsers();
            library.LoadBooks();

            Console.WriteLine("Witaj w systemie bibliotecznym!");
            Console.WriteLine("1. Logowanie");
            Console.WriteLine("2. Rejestracja");
            Console.Write("Wybierz opcję: ");
            string option = Console.ReadLine();

            User user = null;
            if (option == "1")
            {
                Console.Write("Podaj nazwę użytkownika: ");
                string username = Console.ReadLine();
                Console.Write("Podaj hasło: ");
                string password = ReadPassword();
                user = userManager.AuthenticateUser(username, password);
            }
            else if (option == "2")
            {
                Console.Write("Podaj nazwę użytkownika: ");
                string username = Console.ReadLine();
                Console.Write("Podaj hasło: ");
                string password = ReadPassword();
                Console.Write("Powtórz hasło: ");
                string retypedPassword = ReadPassword();
                if (password != retypedPassword)
                {
                    //TODO: in future it will be beter to handle this with Exceptions
                    Console.WriteLine("Hasła nie są takie same!");
                }
                else
                {
                    user = userManager.RegisterUser(username, password);
                }
                
            }

            if (user == null)
            {
                Console.WriteLine("Nie udało się zalogować/zarejestrować.");
                return;
            }

            Console.WriteLine($"Witaj, {user.Username}!");

            while (true)
            {
                Console.WriteLine("\n1. Wypożycz książkę");
                Console.WriteLine("2. Zwrot książki");
                Console.WriteLine("3. Sprawdź swoje wypożyczenia");
                Console.WriteLine("4. Wyświetl dostępne książki");
                Console.WriteLine("4. Wyjście");
                Console.Write("Wybierz opcję: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Podaj tytuł książki: ");
                        string title = Console.ReadLine();
                        library.BorrowBook(user, title);
                        break;
                    case "2":
                        Console.Write("Podaj tytuł książki do zwrotu: ");
                        title = Console.ReadLine();
                        library.ReturnBook(user, title);
                        break;
                    case "3":
                        user.ShowBorrowedBooks();
                        break;
                    case "4":
                        library.ShowAvailableBooks();
                        break;
                    case "5":
                        userManager.SaveUsers();
                        library.SaveBooks();
                        Console.WriteLine("Dziękujemy za korzystanie z systemu!");
                        return;
                    default:
                        Console.WriteLine("Nieprawidłowa opcja!");
                        break;
                }
            }
        }

        static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return password;
        }
        

        

    }
}