namespace Library_App
{
    public class UserNotFoundException : Exception {
        public UserNotFoundException(string message) : base(message) { }
    }

    public class BookNotAvailableException : Exception {
        public BookNotAvailableException(string message) : base(message) { }
    }

    public class OverdueBookException : Exception {
        public OverdueBookException(string message) : base(message) { }
    }
    
    class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException(string message) : base(message) { }
    }

}