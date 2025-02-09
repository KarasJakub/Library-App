namespace Library_App
{
     class User
     {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public List<string> BorrowedBooks { get; set; } = new List<string>();

        public void ShowBorrowedBooks()
        {
            if (BorrowedBooks.Count == 0)
            {
                Console.WriteLine("Brak wypożyczonych książek.");
            }
            else
            {
                Console.WriteLine("Twoje wypożyczone książki:");
                //call to file handler to check all books for username = givenUsername
                foreach (var book in BorrowedBooks)
                {
                    Console.WriteLine("- " + book);
                }
            }
        }
    }
}