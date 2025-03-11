using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego_Jean_Gazon
{
    internal class Personnage
    {
        public abstract class personnage_base
        {
            public String Grade { get; set; }
            public int Force { get; set; }
            public bool Couleur { get; set; } // bleu true rouge false 
            public bool Deplacement { get; set; }// si c'est une bombe ou le drapeau false sinon true 



            public personnage_base(string grade, int force, bool couleur, bool deplacement)
            {
                Grade = grade;
                Force = force;
                Couleur = couleur;
                Deplacement = deplacement;
            }
           
        }
        public class Drapeau : personnage_base
        {
            public Drapeau(bool couleur) : base("Drapeau", 0, couleur, false) { }// on instancie la couleur lors de la création de l'objet 
        }

        public class Bombe : personnage_base
        {
            public Bombe(bool couleur) : base("Bombe", 11, couleur, false) { }
        }

        public class Espion : personnage_base
        {
            public Espion(bool couleur) : base("Espion", 1, couleur, true) { }
        }

        public class Eclaireur : personnage_base
        {
            public Eclaireur(bool couleur) : base("Eclaireur", 2, couleur, true) { }
        }

        public class Demineur : personnage_base
        {
            public Demineur(bool couleur) : base("Démineur", 3, couleur, true) { }
        }

        public class Sergent : personnage_base
        {
            public Sergent(bool couleur) : base("Sergent", 4, couleur, true) { }
        }

        public class Lieutenant : personnage_base
        {
            public Lieutenant(bool couleur) : base("Lieutenant", 5, couleur, true) { }
        }

        public class Capitaine : personnage_base
        {
            public Capitaine(bool couleur) : base("Capitaine", 6, couleur, true) { }
        }

        public class Commandant : personnage_base
        {
            public Commandant(bool couleur) : base("Commandant", 7, couleur, true) { }
        }

        public class Colonel : personnage_base
        {
            public Colonel(bool couleur) : base("Colonel", 8, couleur, true) { }
        }

        public class General : personnage_base
        {
            public General(bool couleur) : base("Général", 9, couleur, true) { }
        }

        public class Marechal : personnage_base
        {
            public Marechal(bool couleur) : base("Maréchal", 10, couleur, true) { }
        }
    }
}
