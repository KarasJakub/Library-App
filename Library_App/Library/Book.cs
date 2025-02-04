namespace Library_App
{
    class Book
    {
        public string Title { get; set; }
        public bool IsAvailable { get; set; } = true;
        public string BorrowedBy { get; set; } = null;
        public DateTime? BorrowedDate { get; set; } = null;
        public DateTime? DueDate { get; set; } = null;
    }
}