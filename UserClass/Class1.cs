using CategoryClass;
namespace UserClass
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public List<Categorie> Categories { get; set; } = new List<Categorie>();
    }
}
