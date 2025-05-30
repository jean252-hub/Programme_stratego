using System;
using System.Drawing;

namespace Stratego_Jean_Gazon
{
    public abstract class personnage_base
    {
        public string Grade { get; set; }
        public int Force { get; set; }
        public bool Couleur { get; set; } // bleu true, rouge false
        public bool Deplacement { get; set; } // false pour bombe/drapeau, true sinon
        public Point PositionGrille { get; set; }

        protected personnage_base(string grade, int force, bool couleur, bool deplacement, Point positionGrille)
        {
            Grade = grade;
            Force = force;
            Couleur = couleur;
            Deplacement = deplacement;
            PositionGrille = positionGrille;
        }
    }

    public class Drapeau : personnage_base
    {
        public Drapeau(bool couleur, Point positionGrille)
            : base("Drapeau", 0, couleur, false, positionGrille) { }
    }

    public class Bombe : personnage_base
    {
        public Bombe(bool couleur, Point positionGrille)
            : base("Bombe", 11, couleur, false, positionGrille) { }
    }

    public class Espion : personnage_base
    {
        public Espion(bool couleur, Point positionGrille)
            : base("Espion", 1, couleur, true, positionGrille) { }
    }

    public class Eclaireur : personnage_base
    {
        public Eclaireur(bool couleur, Point positionGrille)
            : base("Éclaireur", 2, couleur, true, positionGrille) { }
    }

    public class Demineur : personnage_base
    {
        public Demineur(bool couleur, Point positionGrille)
            : base("Démineur", 3, couleur, true, positionGrille) { }
    }

    public class Sergent : personnage_base
    {
        public Sergent(bool couleur, Point positionGrille)
            : base("Sergent", 4, couleur, true, positionGrille) { }
    }

    public class Lieutenant : personnage_base
    {
        public Lieutenant(bool couleur, Point positionGrille)
            : base("Lieutenant", 5, couleur, true, positionGrille) { }
    }

    public class Capitaine : personnage_base
    {
        public Capitaine(bool couleur, Point positionGrille)
            : base("Capitaine", 6, couleur, true, positionGrille) { }
    }

    public class Commandant : personnage_base
    {
        public Commandant(bool couleur, Point positionGrille)
            : base("Commandant", 7, couleur, true, positionGrille) { }
    }

    public class Colonel : personnage_base
    {
        public Colonel(bool couleur, Point positionGrille)
            : base("Colonel", 8, couleur, true, positionGrille) { }
    }

    public class General : personnage_base
    {
        public General(bool couleur, Point positionGrille)
            : base("Général", 9, couleur, true, positionGrille) { }
    }

    public class Marechal : personnage_base
    {
        public Marechal(bool couleur, Point positionGrille)
            : base("Maréchal", 10, couleur, true, positionGrille) { }
    }
}
