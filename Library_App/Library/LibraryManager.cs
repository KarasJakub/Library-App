using System.Text.Json;

namespace Library_App
{
        class Library
    {
        private readonly string _filePath;
        private List<Book> books;
        private const decimal PenaltyPerDay = 1.0m;

        public Library(string filePath)
        {
            this._filePath = filePath;
            books = new List<Book>();
        }

        public void LoadBooks()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                books = JsonSerializer.Deserialize<List<Book>>(json) ?? new List<Book>();
            }
            InitializeBooks();
        }
        private void InitializeBooks()
        {
            //TODO: it propably should be removed in future
            if (books.Count == 0)
            {
                books.AddRange(new List<Book>
                {
                    new Book { Title = "Władca Pierścieni", IsAvailable = true },
                    new Book { Title = "Hobbit", IsAvailable = true },
                    new Book { Title = "Duma i uprzedzenie", IsAvailable = true }
                });
                SaveBooks();
            }
        }
        
        
        public void ShowAvailableBooks()
        {
            var availableBooks = books.Where(b => b.IsAvailable).ToList();

            if (availableBooks.Count == 0)
            {
                //TODO: Exception in future
                Console.WriteLine("Brak dostępnych książek.");
            }
            else
            {
                Console.WriteLine("Dostępne książki:");
                foreach (var book in availableBooks)
                {
                    Console.WriteLine($"- {book.Title}");
                }
            }
        }

        public void SaveBooks()
        {
            string json = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public void BorrowBook(User user, string title)
        {
            var book = books.FirstOrDefault(b => b.Title == title && b.IsAvailable);
            if (book != null)
            {
                book.IsAvailable = false;
                book.BorrowedBy = user.Username;
                book.BorrowedDate = DateTime.Now;
                book.DueDate = DateTime.Now.AddDays(30);
                user.BorrowedBooks.Add(book.Title);
                SaveBooks();
                Console.WriteLine($"Książka wypożyczona. Termin zwrotu: {book.DueDate.Value:dd-MM-yyyy}");
            }
            else
            {
                Console.WriteLine("Książka niedostępna.");
            }
        }
        public void RemoveBook(string title)
        {
            var book = books.FirstOrDefault(b => b.Title == title);
            if (book != null)
            {
                books.Remove(book);
                SaveBooks();
                Console.WriteLine("Książka została usunięta.");
            }
            else
            {
                Console.WriteLine("Nie znaleziono książki o podanym tytule.");
            }
        }
        public void ReturnBook(User user, string title)
        {
            var book = books.FirstOrDefault(b => b.Title == title && !b.IsAvailable && b.BorrowedBy == user.Username);
            if (book != null)
            {
                DateTime today = DateTime.Now;
                if (book.DueDate.HasValue && today > book.DueDate.Value)
                {
                    int daysLate = (today - book.DueDate.Value).Days;
                    decimal penalty = daysLate * PenaltyPerDay;
                    Console.WriteLine($"Przekroczyłeś termin zwrotu o {daysLate} dni. Naliczona kara: {penalty} zł");
                }
                book.IsAvailable = true;
                book.BorrowedBy = null;
                book.BorrowedDate = null;
                book.DueDate = null;
                user.BorrowedBooks.Remove(title);
                SaveBooks();
                Console.WriteLine("Książka zwrócona.");
            }
            else
            {
                Console.WriteLine("Nie posiadasz tej książki.");
            }
        }
    }
}