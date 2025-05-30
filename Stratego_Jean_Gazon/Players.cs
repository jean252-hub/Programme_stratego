using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Stratego_Jean_Gazon
{
    public enum Player
    {
        Player_Blue = 1,
        Player_Red = 2,
    }

    public class Players
    {
        public Player CurrentPlayer { get; private set; } // Joueur actuel

        public Dictionary<Point, personnage_base> PositionsPionsBleus { get; private set; } // Pions bleus
        public Dictionary<Point, personnage_base> PositionsPionsRouges { get; private set; } // Pions rouges

        public Players()
        {
            CurrentPlayer = Player.Player_Blue; // Le joueur bleu commence
            PositionsPionsBleus = new Dictionary<Point, personnage_base>();
            PositionsPionsRouges = new Dictionary<Point, personnage_base>();
        }

        public void Initialisation_Jeu(Grille_Manager grille, Button Button_Pret, FicJeu winjeu)
        {
            Button_Pret.Visible = true;

            Button_Pret.Click += async (sender, e) =>
            {
                grille.TerminerPlacement();
                ChangerJoueur();

                if (CurrentPlayer == Player.Player_Blue)
                {
                    grille.Cacher_Piece(true);
                    grille.ActiverPlacement(CurrentPlayer);
                    await ((FicJeu)winjeu).transitionManager.ShowPlacement(CurrentPlayer);
                }
                else if (CurrentPlayer == Player.Player_Red)
                {
                    grille.Cacher_Piece(false);
                    grille.ActiverPlacement(CurrentPlayer);
                    await ((FicJeu)winjeu).transitionManager.ShowPlacement(CurrentPlayer);
                }
                else
                {
                    Button_Pret.Visible = false;
                }
            };

            grille.Cacher_Piece(true);
            grille.ActiverPlacement(CurrentPlayer);
        }

        public void InitialisationFin(Grille_Manager grille)
        {
            grille.TerminerPlacement();
        }

        public void ChangerJoueur()
        {
            CurrentPlayer = (CurrentPlayer == Player.Player_Red) ? Player.Player_Blue : Player.Player_Red;
        }

        public void AjouterPion(Point position, personnage_base pion)
        {
            if (CurrentPlayer == Player.Player_Blue)
            {
                if (!PositionsPionsBleus.ContainsKey(position))
                {
                    PositionsPionsBleus[position] = pion;
                }
            }
            else if (CurrentPlayer == Player.Player_Red)
            {
                if (!PositionsPionsRouges.ContainsKey(position))
                {
                    PositionsPionsRouges[position] = pion;
                }
            }
        }

        public void SupprimerPion(Point position)
        {
            if (CurrentPlayer == Player.Player_Blue)
            {
                PositionsPionsBleus.Remove(position);
            }
            else if (CurrentPlayer == Player.Player_Red)
            {
                PositionsPionsRouges.Remove(position);
            }
        }

        public void MemoriserPositionPion(Panel panel, int caseX, int caseY, personnage_base pion)
        {
            int largeurCase = panel.Width / 10;
            int hauteurCase = panel.Height / 10;
            int positionX = caseX * largeurCase;
            int positionY = caseY * hauteurCase;
            pion.PositionGrille = new Point(positionX, positionY);
            AjouterPion(new Point(positionX, positionY), pion);
        }

        public Dictionary<Point, personnage_base> ObtenirPositionsBleues()
        {
            return new Dictionary<Point, personnage_base>(PositionsPionsBleus);
        }

        public Dictionary<Point, personnage_base> ObtenirPositionsRouges()
        {
            return new Dictionary<Point, personnage_base>(PositionsPionsRouges);
        }
    }
}
