using System.Collections.Generic;
using System.Linq;
using Services;
using UserClass;

namespace Services // Keep it in the same namespace as DataService
{
    public class UserService
    {
        private const string FilePath = "users.json";
        private List<User> _users;

        // This keeps track of whoever just logged in
        public User? CurrentUser { get; private set; }

        public UserService()
        {
            // Load the list of all users from the JSON file
            // If the file doesn't exist, start with an empty list
            List<User> loadedUsers = DataService.Load<List<User>>("users.json");

            // 2. Explicit 'if' instead of '??'
            if (loadedUsers != null)
            {
                // If the file was read successfully, use that list
                _users = loadedUsers;
            }
            else
            {
                // If the file doesn't exist or is empty, create a fresh list
                _users = new List<User>();
            }
        }

        public bool Login(string username, string password)
        {
            // Look for a user with the matching name and password
            var user = _users.FirstOrDefault(u =>
                u.Username == username && u.Password == password);

            if (user != null)
            {
                CurrentUser = user;
                return true;
            }

            return false;
        }

        public bool Register(string username, string password)
        {
            // Check if someone already has this name
            if (_users.Any(u => u.Username == username))
            {
                return false;
            }

            // Create the new user object
            var newUser = new User
            {
                Username = username,
                Password = password,
                // Initialize the list so it's not null
                Categories = new List<CategoryClass.Categorie>()
            };

            _users.Add(newUser);
            CurrentUser = newUser; // Auto-log them in
            Save();
            return true;
        }

        public void Save()
        {
            // Write the entire list of users back to the file
            DataService.Save(FilePath, _users);
        }
    }
}