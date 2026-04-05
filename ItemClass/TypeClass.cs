using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modeles
{
    public class TypeClass
    {
        public string Nom { get; set; }

        public TypeClass() { }

        public TypeClass(string nom)
        {
            this.Nom = nom;
        }

        public override string ToString()
        {
            return Nom;
        }
    }
}
