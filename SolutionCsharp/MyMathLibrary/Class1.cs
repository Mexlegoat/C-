using System;

namespace MyMathLibrary
{
    public static class MathHelpers
    {
        public static double SurfaceCarre(double cote)
        {
            return cote * cote;
        }

        public static double SurfaceRectangle(double largeur, double hauteur)
        {
            return largeur * hauteur;
        }

        public static double SurfaceCercle(double rayon)
        {
            return Math.PI * rayon * rayon;
        }

        public static bool EstComprisEntre(double valeur, double min, double max)
        {
            return valeur >= min && valeur <= max;
        }

        public static double Moyenne(double a, double b)
        {
            return (a + b) / 2.0;
        }

        public static long Factorielle(int n)
        {
            if (n < 0)
                throw new ArgumentException("n doit être positif");

            long resultat = 1;
            for (int i = 1; i <= n; i++)
            {
                resultat *= i;
            }
            return resultat;
        }
    }
}