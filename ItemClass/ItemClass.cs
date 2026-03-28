
namespace ItemClass
{
    public class Item
    {
        public string Nom { get; set; }
        public string Chemin { get; set; }
        public string Genre { get; set; }
        public string IconPath { get; set; }
        public override string ToString()
        {
            return Nom;
        }
    }
}
