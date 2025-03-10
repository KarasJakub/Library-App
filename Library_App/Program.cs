﻿namespace Library_App
{
    class Program
    {
        static void Main()
        {
            try {
            LibraryManager library = new LibraryManager(Path.Combine(
                Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Data")),
                "library.json"
            ));
            UserAuthManager userManager = new UserAuthManager(Path.Combine(
                Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Data")),
                "users.json"
            ));
            userManager.LoadUsers();
            library.LoadBooks();

            Console.WriteLine("Witaj w systemie bibliotecznym!");
            Console.WriteLine("1. Logowanie");
            Console.WriteLine("2. Rejestracja");
            Console.Write("Wybierz opcję: ");
            string option = Console.ReadLine();

            User user = null;
            if (option == "1") {
                Console.Write("Podaj nazwę użytkownika: ");
                string username = Console.ReadLine();
                Console.Write("Podaj hasło: ");
                string password = ReadPassword();
                user = userManager.AuthenticateUser(username, password);
            }
            else if (option == "2") {
                Console.Write("Podaj nazwę użytkownika: ");
                string username = Console.ReadLine();
                Console.Write("Podaj hasło: ");
                string password = ReadPassword();
                Console.Write("Powtórz hasło: ");
                string retypedPassword = ReadPassword();
                if (password != retypedPassword) {
                    Console.WriteLine("Hasła nie są takie same!");
                }
                else {
                    user = userManager.RegisterUser(username, password);
                }
                
            }
            if (user == null) {
                Console.WriteLine("Nie udało się zalogować/zarejestrować.");
                return;
            }

            Console.WriteLine($"Witaj, {user.Username}!");

            while (true) {
                try { 
                Console.WriteLine("\n1. Wypożycz książkę");
                Console.WriteLine("2. Zwrot książki");
                Console.WriteLine("3. Sprawdź swoje wypożyczenia");
                Console.WriteLine("4. Wyświetl dostępne książki");
                Console.WriteLine("5. Wyjście");
                if (user.IsAdmin) {
                    Console.WriteLine("Opcje administracyjne:");
                    Console.WriteLine("6. Dodaj książkę");
                    Console.WriteLine("7. Usuń książkę");
                    Console.WriteLine("8. Usuń użytkownika");
                }
                Console.Write("Wybierz opcję: ");
                string choice = Console.ReadLine();

                switch (choice) {
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
                        break;
                    case "6":
                        if (user.IsAdmin) {
                            Console.Write("Podaj tytuł książki do dodania: ");
                            title = Console.ReadLine();
                            library.AddBook(title);
                        }
                        break;
                    case "7":
                        if (user.IsAdmin) {
                            Console.Write("Podaj tytuł książki do usunięcia: ");
                            title = Console.ReadLine();
                            library.RemoveBook(title);
                        }
                        break;
                    case "8":
                        if (user.IsAdmin) {
                            Console.Write("Podaj nazwę użytkownika do usunięcia: ");
                            string usernameToRemove = Console.ReadLine();
                            userManager.RemoveUser(usernameToRemove);
                        }
                        break;
                    default:
                        Console.WriteLine("Nieprawidłowa opcja!");
                        break;
                }
                }
                catch (UserNotFoundException ex) {
                    Console.WriteLine($"Błąd: {ex.Message}");
                }
                catch (BookNotAvailableException ex) {
                    Console.WriteLine($"Błąd: {ex.Message}");
                }
                catch (OverdueBookException ex) {
                    Console.WriteLine($"Błąd: {ex.Message}");
                }
                catch (Exception ex) {
                    Console.WriteLine($"Wystąpił błąd: {ex.Message}");
                }
            }
        }
        catch (Exception ex) {
            Console.WriteLine($"Błąd krytyczny: {ex.Message}");
        }
            static string ReadPassword() {
                string password = "";
                ConsoleKeyInfo key;
                do {
                    key = Console.ReadKey(true);
                    if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter) {
                        password += key.KeyChar;
                        Console.Write("*");
                    }
                    else if (key.Key == ConsoleKey.Backspace && password.Length > 0) {
                        password = password.Substring(0, password.Length - 1);
                        Console.Write("\b \b");
                    }
                } while (key.Key != ConsoleKey.Enter);

                Console.WriteLine();
                return password;
            }
        }
    }
}