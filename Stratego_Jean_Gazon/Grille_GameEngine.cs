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

        public bool Deplacer_Personnage(personnage_base personnage, int NewPositionX, int NewPositionY)
        {
            if (personnage.Deplacement != true) { return false; }
            return false ;
        }
    
    }
}
