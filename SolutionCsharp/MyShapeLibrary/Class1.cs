using MyShapeLibrary;
using System.Reflection.Metadata.Ecma335;

namespace MyShapeLibrary
{
    public class Cordonnee
    {
        private int x;
        private int y;

        public int X
        {
            get { return x; }
            set { x = value; }
        }
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        public Cordonnee(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Cordonnee() : this(0, 0)
        {
        }
        public override string ToString()
        {
            return "(" + x + "," + y + ")";
        }
    }
    
    public abstract class Forme
    {
        private Cordonnee pointAccroche;

        public Cordonnee PointAccroche
        {
            get { return pointAccroche; }
            set { pointAccroche = value; }
        }
        
        public Forme()
        {
            pointAccroche = new Cordonnee();
        }

        public Forme(Cordonnee c)
        {
            pointAccroche = c;
        }
    }
    public class Carre : Forme, IEstDans, IPolygone
    {
        private int longueur;
        public int Longueur
        {
            get { return longueur; }
            set { longueur = value; }
        }

        public Carre() : this(new Cordonnee(),0)
        {
        }
        public Carre(Cordonnee pos, int longueur): base(pos)
        {
            this.longueur = longueur;
        }
        public override string ToString()
        {
            return $"Carre: Centre: {PointAccroche}, Longueur: {Longueur}";
        }
        public bool CordonneeEstDans(Cordonnee p)
        {
            return p.X >= PointAccroche.X &&
                p.X <= (PointAccroche.X + Longueur) &&
                p.Y >= PointAccroche.Y &&
                p.Y <= PointAccroche.Y + Longueur;
        }
        public int NbSommets
        {
            get { return 4; }
        }
    }
    public class Cercle : Forme, IEstDans
    {
        private int rayon;
        public int Rayon
        {
            get { return rayon; }
            set { rayon = value; }
        }
        public int Diametre
        {
            get { return rayon * 2; }
        }
        public Cercle() : this(new Cordonnee(), 0)
        {
        }
        public Cercle(Cordonnee c, int r) : base(c)
        {
            this.rayon = r;
        }
        public override string ToString()
        {
            return $"Cercle: Centre: {PointAccroche}, Rayon: {Rayon}, Diametre: {Diametre}";
        }
        public bool CordonneeEstDans(Cordonnee p)
        {
            int dx = p.X - PointAccroche.X;
            int dy = p.Y - PointAccroche.Y;
            
            return (dx * dx  + dy * dy) < (Rayon * Rayon);
        }
    }
    public class Rectangle : Forme, IEstDans, IPolygone
    {
        private int longueur, largeur;
        public int Longueur
        {
            get { return longueur; }
            set {  longueur = value; }
        }
        public int Largeur
        {
            get { return largeur; }
            set {  largeur = value; }
        }
        public Rectangle() : this(new Cordonnee(), 0, 0)
        {
        }
        public Rectangle(Cordonnee c, int l,int L) : base(c)
        {
            this.largeur = l;
            this.longueur = L;
        }
        public override string ToString()
        {
            return $"Rectangle: Rectangle: {PointAccroche}, Largeur: {Largeur}, Longueur: {Longueur}";
        }
        public bool CordonneeEstDans(Cordonnee p)
        {
            return p.X >= PointAccroche.X &&
                p.X <= PointAccroche.X + Largeur &&
                p.Y >= PointAccroche.Y &&
                p.Y <= PointAccroche.Y + Longueur;

        }
        public int NbSommets
        {
            get { return 4; }
        }
    }
    public interface IEstDans
    {
        bool CordonneeEstDans(Cordonnee p);
    }
    public interface IPolygone
    {
        public int NbSommets { get;}
    }

}

