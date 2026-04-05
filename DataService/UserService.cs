using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Modeles;
using Services;
using UserClass;

namespace Services 
{
    public class UserService
    {
        private const string FilePath = "users.json";
        private List<User> _users;

        public User? CurrentUser { get; private set; }

        public UserService()
        {
            List<User> loadedUsers = DataService.Load<List<User>>("users.json");

            if (loadedUsers != null)
            {
                _users = loadedUsers;
            }
            else
            {
                _users = new List<User>();
            }
        }

        public bool Login(string username, string password)
        {
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
            if (_users.Any(u => u.Username == username))
            {
                return false;
            }

            var newUser = new User
            {
                Username = username,
                Password = password,

                Categories = new List<CategoryClass.Categorie>(),
                UserCreatedTypes = new List<TypeClass>() 
            };

            _users.Add(newUser);
            CurrentUser = newUser;
            Save();
            return true;
        }

        public void Save()
        {
            
            DataService.Save(FilePath, _users);
        }
    }
}