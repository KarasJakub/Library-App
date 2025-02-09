using System.Text.Json;
using System.Security.Cryptography;

namespace Library_App
{
    class UserAuthManager
    {
        private string filePath;
        private List<User> users;

        public UserAuthManager(string filePath)
        {
            this.filePath = filePath;
            users = new List<User>();
        }

        public void LoadUsers()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            else
            {
                //generating dummy data
                users = new List<User>
                {
                    new User { Username = "admin", PasswordHash = ComputeHash("admin123") },
                    new User { Username = "user", PasswordHash = ComputeHash("user123") }
                };
                SaveUsers();
            }
        }

        public void SaveUsers()
        {
            string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public User AuthenticateUser(string username, string password)
        {
            string hash = ComputeHash(password);
            return users.FirstOrDefault(u => u.Username == username && u.PasswordHash == hash);
        }

        public User RegisterUser(string username, string password)
        {
            //Checks if users contain given user
            if (users.Any(u => u.Username == username))
            {
                Console.WriteLine("Użytkownik o takiej nazwie już istnieje.");
                return null;
            }
            var newUser = new User { Username = username, PasswordHash = ComputeHash(password) };
            users.Add(newUser);
            SaveUsers();
            return newUser;
        }

        public static string ComputeHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(bytes);
            }
        }
        
        public void RemoveUser(string username)
        {
            var user = users.FirstOrDefault(u => u.Username == username);
            if (user != null && !user.IsAdmin)
            {
                users.Remove(user);
                SaveUsers();
                Console.WriteLine("Użytkownik został usunięty.");
            }
            else
            {
                Console.WriteLine("Nie znaleziono użytkownika lub nie można usunąć administratora.");
            }
        }
    }
}