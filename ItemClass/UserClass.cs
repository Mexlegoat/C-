using CategoryClass;
using Modeles;

namespace UserClass
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public List<Categorie> Categories { get; set; } = new List<Categorie>();
        public List<TypeClass> UserCreatedTypes { get; set; } = new List<TypeClass>();
        public UserSettings Preferences { get; set; } = new UserSettings();
    }
    public class UserSettings
    {
        public bool IsDarkMode { get; set; } = false;
        public bool ShowType { get; set; } = false;
        public bool ShowGenre { get; set; } = false;
        public bool DoubleClickToExecute { get; set; } = true;
        public string DefaultBrowsePath { get; set; } = "C:\\";
    }
}
