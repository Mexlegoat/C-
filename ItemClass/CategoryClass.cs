
using ItemClass;

namespace CategoryClass
{
    public class Categorie
    {
        public string Nom {  get; set; }

        public List<Item> Items { get; set; } = new List<Item> { };
    }
}
