
using System.Diagnostics;
using System.Text.Json.Serialization;
using Modeles;
namespace ItemClass
{
    [JsonDerivedType(typeof(Jeu), typeDiscriminator: "jeu")]
    [JsonDerivedType(typeof(Travail), typeDiscriminator: "travail")]
    [JsonDerivedType(typeof(Multimedia), typeDiscriminator: "multimedia")]
    public abstract class Item : ILaunchable
    {
        public static int nextId = 1;
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Chemin { get; set; }
        public string IconPath { get; set; }
        public DateTime DateAjoute { get; set; }
        public Type CustomType { get; set; }
        public Item()
        {
        }
        public void InitializeNewItem()
        {
            this.Id = nextId;
            nextId = nextId + 1;
            this.DateAjoute = DateTime.Now;
        }

        public static void SetNextId(int newId)
        {
            nextId = newId;
        }
        public override string ToString()
        {
            return Nom;
        }
        public virtual void Launch()
        {
            if (string.IsNullOrWhiteSpace(this.Chemin) == false)
            {
                // 2. Only start the process if we have a real path
                Process.Start(new ProcessStartInfo
                {
                    FileName = this.Chemin,
                    UseShellExecute = true
                });
            }
        }
    }
    public class Jeu : Item
    {
        public string Genre { get; set; }
        public override string ToString() { return Genre; }
        public Jeu() : base()
        {
        }
    }
    public class Travail : Item
    {
        public string Langage { get; set; }
        public override string ToString() { return Langage; }
        public Travail() : base()
        {
        }
    }
    public class Multimedia : Item
    {
        public string Genre { get; set; }
        public override string ToString() { return Genre; }
        public Multimedia() : base()
        {
        }
    }
    public interface ILaunchable
    {
        string Nom { get; set; }
        string Chemin { get; set; }

        void Launch();
    }
}
