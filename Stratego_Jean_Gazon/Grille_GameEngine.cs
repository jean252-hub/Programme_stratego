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

        public byte ResoudreAffrontement(personnage_base attaquant, personnage_base defenseur)
        {
            int forceAttaquant = attaquant.Force;
            int forceDefenseur = defenseur.Force;

            // Bombe déminée
            if (forceAttaquant == 3 && forceDefenseur == 11)
            {
                System.Diagnostics.Debug.WriteLine("bombe déminée");
                System.Windows.Forms.MessageBox.Show("bombe deminée");
                return 1;
            }
            // Espion attaque Maréchal
            if (forceAttaquant == 1 && forceDefenseur == 10)
            {
                System.Diagnostics.Debug.WriteLine("Attaquant gagne ! (Espion attaque Maréchal)");
                return 1;
            }
            // Drapeau touché
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
            {
                System.Diagnostics.Debug.WriteLine("égalité !");
                return 3;
            }
        }
    }
}
