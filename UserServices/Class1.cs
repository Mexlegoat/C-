using UserClass;
using DataService;
public class UserService
{
    private string filePath = "users.json";
    private List<User> users;

    public User CurrentUser { get; private set; }

    public UserService()
    {
        users = DataService.Load<List<User>>(filePath) ?? new List<User>();
    }

    public bool Login(string username, string password)
    {
        var user = users.FirstOrDefault(u =>
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
        if (users.Any(u => u.Username == username))
            return false;

        var newUser = new User
        {
            Username = username,
            Password = password
        };

        users.Add(newUser);
        Save();
        return true;
    }

    public void Save()
    {
        DataService.Save(filePath, users);
    }
}