using System.Text.Json;
using System.Security.Cryptography;

namespace Library_App {
    class UserAuthManager {
        private string filePath;
        private List<User> users;

        public UserAuthManager(string filePath) {
            this.filePath = filePath;
            users = new List<User>();
        }

        public void LoadUsers() {
            if (File.Exists(filePath)) {
                string json = File.ReadAllText(filePath);
                users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            else {
                //Seeding databse if empty
                users = new List<User>
                {
                    new User { Username = "admin", PasswordHash = ComputeHash("admin") },
                    new User { Username = "user", PasswordHash = ComputeHash("user") }
                };
                SaveUsers();
            }
        }

        public void SaveUsers() {
            string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public User AuthenticateUser(string username, string password)
        {
            string hash = ComputeHash(password);
            var user = users.FirstOrDefault(u => u.Username == username && u.PasswordHash == hash);

            if (user == null)
            {
                throw new UserNotFoundException("Nie znaleziono użytkownika, lub podango błędne haslo");
            }

            return user;
        }

        public User RegisterUser(string username, string password) {
            if (users.Any(u => u.Username == username))
            {
                throw new UserAlreadyExistsException("Użytkownik o takiej nazwie już istnieje.");
            }
            var newUser = new User { Username = username, PasswordHash = ComputeHash(password) };
            users.Add(newUser);
            SaveUsers();
            return newUser;
        }

        private static string ComputeHash(string input) {
            using (SHA256 sha256 = SHA256.Create()) {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(bytes);
            }
        }
        
        public void RemoveUser(string username) {
            var user = users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                throw new UserNotFoundException("Nie znaleziono użytkownika");
            }

            if (user.IsAdmin)
            {
                throw new InvalidOperationException("Nie można usunąć administratora!");
            }
            users.Remove(user);
            SaveUsers();
            Console.WriteLine("Użytkownik został usunięty.");
        }
    }
}