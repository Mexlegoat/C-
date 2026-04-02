using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modeles
{
    public class TypeClass
    {
        // On utilise souvent "Nom" pour plus de clarté dans le Binding
        public string Nom { get; set; }

        // Il est fortement conseillé d'avoir un constructeur vide pour la désérialisation JSON
        public TypeClass() { }

        public TypeClass(string nom)
        {
            this.Nom = nom;
        }

        // Optionnel : Override ToString pour faciliter le débogage
        public override string ToString()
        {
            return Nom;
        }
    }
}
