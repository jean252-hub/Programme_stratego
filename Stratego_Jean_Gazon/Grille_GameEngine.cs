using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego_Jean_Gazon
{
    internal class Grille_GameEngine
    {
        private personnage_base[,] grille = new personnage_base[10, 10];
        /*
        public bool Deplacer_Personnage(personnage_base personnage, int NewPositionX, int NewPositionY)
        {
            if (personnage.Deplacement != true) { return false; }
            return false ;
        }*/
        public byte ResoudreAffrontement(string nomAttaquant, string nomDefenseur)
        {
            int forceAttaquant = GetForce(nomAttaquant);
            int forceDefenseur = GetForce(nomDefenseur);

            if (forceAttaquant == 3 && forceDefenseur == 11)
            {
                // Bombe déminée
                System.Diagnostics.Debug.WriteLine("bombe déminée");
                System.Windows.Forms.MessageBox.Show("bombe deminée");
                return 1;
            }
            if (forceAttaquant == 1 && forceDefenseur == 10)
            {
                System.Diagnostics.Debug.WriteLine("Attaquant gagne ! (Espion attaque Maréchal)");
                              return 1;
            }
            if (forceDefenseur == 0)
            {
                System.Diagnostics.Debug.WriteLine("Attaquant gagne ! (Drapeau touché)");
                return 4;
            }
            if (forceAttaquant > forceDefenseur)
            {
                System.Diagnostics.Debug.WriteLine("Attaquant gagne !");
                return 1;
            }
            else if (forceAttaquant < forceDefenseur)
            {
                System.Diagnostics.Debug.WriteLine("Défenseur gagne !");
                return 2;
            }
            else
                System.Diagnostics.Debug.WriteLine("égalité !");
            return 3;
        }

        private int GetForce(string nom)
        {
            switch (nom)
            {
                case "Maréchal": return 10;
                case "Général": return 9;
                case "Colonel": return 8;
                case "Commandant": return 7;
                case "Capitaine": return 6;
                case "Lieutenant": return 5;
                case "Sergent": return 4;
                case "Démineur": return 3;
                case "Éclaireur": return 2;
                case "Espion": return 1;
                case "Bombe": return 11;
                case "Drapeau": return 0;
                default: return 0;
            }
        }


    }
}
