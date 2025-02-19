namespace Library_App
{
     class User {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public bool IsAdmin { get; set; } = false;
        public List<string> BorrowedBooks { get; set; } = new List<string>();
        
        public void ShowBorrowedBooks() {
            if (BorrowedBooks.Count == 0) {
                Console.WriteLine("Brak wypożyczonych książek.");
            }
            else {
                Console.WriteLine("Twoje wypożyczone książki:");
                foreach (var book in BorrowedBooks) {
                    Console.WriteLine("- " + book);
                }
            }
        }
    }
}