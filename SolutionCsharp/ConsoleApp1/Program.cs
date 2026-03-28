using System;
using System.Collections.Generic;
using System.Linq;
using MyShapeLibrary;

class Program
{
    static void Main()
    {
        // -------------------------------------------------
        // 1️⃣ Création de 2 objets de chaque sorte
        // -------------------------------------------------

        Carre c1 = new Carre(new Cordonnee(0, 0), 4);
        Carre c2 = new Carre();
        c2.Longueur = 2;
        c2.PointAccroche = new Cordonnee(5, 5);

        Cercle ce1 = new Cercle(new Cordonnee(2, 2), 3);
        Cercle ce2 = new Cercle();
        ce2.Rayon = 5;
        ce2.PointAccroche = new Cordonnee(10, 10);

        Rectangle r1 = new Rectangle(new Cordonnee(1, 1), 3, 6);
        Rectangle r2 = new Rectangle();
        r2.Largeur = 2;
        r2.Longueur = 8;
        r2.PointAccroche = new Cordonnee(7, 3);

        Console.WriteLine("---- Objets créés ----");
        Console.WriteLine(c1);
        Console.WriteLine(c2);
        Console.WriteLine(ce1);
        Console.WriteLine(ce2);
        Console.WriteLine(r1);
        Console.WriteLine(r2);

        // -------------------------------------------------
        // 2️⃣ Liste générique List<Forme>
        // -------------------------------------------------

        List<Forme> formes = new List<Forme>
        {
            c1, c2, ce1, ce2, r1, r2
        };

        Console.WriteLine("\n---- Liste complète ----");
        foreach (Forme f in formes)
        {
            Console.WriteLine(f);
        }

        // -------------------------------------------------
        // 3️⃣ Polygones / Non Polygones
        // -------------------------------------------------

        Console.WriteLine("\n---- Polygones ----");
        foreach (Forme f in formes)
        {
            if (f is IPolygone)
                Console.WriteLine(f);
        }

        Console.WriteLine("\n---- Non Polygones ----");
        foreach (Forme f in formes)
        {
            if (f is not IPolygone)
                Console.WriteLine(f);
        }

        // -------------------------------------------------
        // 4️⃣ Liste de 5 carrés
        // -------------------------------------------------

        List<Carre> carres = new List<Carre>
        {
            new Carre(new Cordonnee(1,1),5),
            new Carre(new Cordonnee(2,2),2),
            new Carre(new Cordonnee(3,3),4),
            new Carre(new Cordonnee(4,4),2),
            new Carre(new Cordonnee(5,5),7)
        };

        Console.WriteLine("\n---- Liste carrés ----");
        carres.ForEach(c => Console.WriteLine(c));

        // 🔹 Tri croissant par longueur (sans CompareTo)
        carres = carres.OrderBy(c => c.Longueur).ToList();

        Console.WriteLine("\n---- Tri par taille croissante ----");
        carres.ForEach(c => Console.WriteLine(c));

        // 🔹 Tri par abscisse (X)
        carres = carres.OrderBy(c => c.PointAccroche.X).ToList();

        Console.WriteLine("\n---- Tri par abscisse croissante ----");
        carres.ForEach(c => Console.WriteLine(c));

        // -------------------------------------------------
        // 5️⃣ Recherches
        // -------------------------------------------------

        int reference = 2;

        var premier = carres.Find(c => c.Longueur == reference);
        int indexPremier = carres.FindIndex(c => c.Longueur == reference);

        var dernier = carres.FindLast(c => c.Longueur == reference);
        int indexDernier = carres.FindLastIndex(c => c.Longueur == reference);

        var tous = carres.FindAll(c => c.Longueur == reference);

        Console.WriteLine($"\nPremier trouvé à l'index {indexPremier}");
        Console.WriteLine($"Dernier trouvé à l'index {indexDernier}");
        Console.WriteLine($"Nombre total trouvés : {tous.Count}");

        // -------------------------------------------------
        // 6️⃣ Exists
        // -------------------------------------------------

        Console.WriteLine("\nExiste longueur 7 ?");
        Console.WriteLine(carres.Exists(c => c.Longueur == 7));

        // -------------------------------------------------
        // 7️⃣ Contains (égalité simple)
        // -------------------------------------------------

        Carre carreRef = new Carre(new Cordonnee(5, 5), 7);

        Console.WriteLine("\nContient le carré (5,5,7) ?");
        Console.WriteLine(carres.Any(c =>
            c.Longueur == carreRef.Longueur &&
            c.PointAccroche.X == carreRef.PointAccroche.X &&
            c.PointAccroche.Y == carreRef.PointAccroche.Y));

        // -------------------------------------------------
        // 8️⃣ Formes contenant un point
        // -------------------------------------------------

        Cordonnee point = new Cordonnee(2, 2);

        Console.WriteLine("\n---- Formes contenant le point (2,2) ----");
        foreach (Forme f in formes)
        {
            if (f is IEstDans test && test.CordonneeEstDans(point))
                Console.WriteLine(f);
        }

        // -------------------------------------------------
        // 9️⃣ Tri des formes par surface (sans modifier classes)
        // -------------------------------------------------

        var triSurface = formes.OrderBy(f =>
        {
            if (f is Carre c) return c.Longueur * c.Longueur;
            if (f is Rectangle r) return r.Longueur * r.Largeur;
            if (f is Cercle ce) return Math.PI * ce.Rayon * ce.Rayon;
            return 0;
        });

        Console.WriteLine("\n---- Tri par surface ----");
        foreach (var f in triSurface)
        {
            Console.WriteLine(f);
        }
    }
}