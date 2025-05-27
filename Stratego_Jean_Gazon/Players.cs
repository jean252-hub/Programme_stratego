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

        public Dictionary<Point, string> PositionsPionsBleus { get; private set; } // Positions des pions de l'équipe bleue
        public Dictionary<Point, string> PositionsPionsRouges { get; private set; } // Positions des pions de l'équipe rouge

        public Players()
        {
            CurrentPlayer = Player.Player_Blue; // Le joueur bleu commence
            PositionsPionsBleus = new Dictionary<Point, string>();
            PositionsPionsRouges = new Dictionary<Point, string>();
        }

        public void Initialisation_Jeu(Grille_Manager grille, Button Button_Pret, FicJeu winjeu)
        {
            // Rendre le bouton visible pour démarrer la phase de placement
            Button_Pret.Visible = true;

            // Attacher un gestionnaire d'événements au clic du bouton
            Button_Pret.Click += (sender, e) =>
            {
                // Terminer le placement pour le joueur actuel
                grille.TerminerPlacement();

                // Passer au joueur suivant
                ChangerJoueur();

                // Vérifier quel joueur est actif et activer le placement en conséquence
                if (CurrentPlayer == Player.Player_Blue)
                {
                    MessageBox.Show("C'est au joueur bleu de placer ses pions !");
                    grille.Cacher_Piece(true); // Cacher les pions du joueur rouge
                    grille.ActiverPlacement(CurrentPlayer);
                }
                else if (CurrentPlayer == Player.Player_Red)
                {
                    MessageBox.Show("C'est au joueur rouge de placer ses pions !");
                    grille.Cacher_Piece(false); // Cacher les pions du joueur bleu
                    grille.ActiverPlacement(CurrentPlayer);
                }
                else
                {
                    // Si aucune phase de placement n'est active, masquer le bouton
                    MessageBox.Show("Phase de placement terminée !");
                    Button_Pret.Visible = false;
                }
            };

            // Activer le placement initial pour le joueur bleu
            grille.Cacher_Piece(true); // Cacher les pions du joueur rouge
            grille.ActiverPlacement(CurrentPlayer);
            MessageBox.Show("C'est au joueur bleu de commencer à placer ses pions !");
        }

        public void InitialisationFin(Grille_Manager grille)
        {
            grille.TerminerPlacement();
        }

        public void ChangerJoueur()
        {
            CurrentPlayer = (CurrentPlayer == Player.Player_Red) ? Player.Player_Blue : Player.Player_Red;
        }

        public void AjouterPion(Point position, string typePion)
        {
            if (CurrentPlayer == Player.Player_Blue)
            {
                if (!PositionsPionsBleus.ContainsKey(position))
                {
                    PositionsPionsBleus[position] = typePion;
                }
            }
            else if (CurrentPlayer == Player.Player_Red)
            {
                if (!PositionsPionsRouges.ContainsKey(position))
                {
                    PositionsPionsRouges[position] = typePion;
                }
            }
        }

        public void SupprimerPion(Point position)
        {
            if (CurrentPlayer == Player.Player_Blue)
            {
                if (PositionsPionsBleus.ContainsKey(position))
                {
                    PositionsPionsBleus.Remove(position);
                }
            }
            else if (CurrentPlayer == Player.Player_Red)
            {
                if (PositionsPionsRouges.ContainsKey(position))
                {
                    PositionsPionsRouges.Remove(position);
                }
            }
        }

        public void MemoriserPositionPion(Panel panel, int caseX, int caseY, string typePion)
        {
            // Calculer la largeur et la hauteur d'une case
            int largeurCase = panel.Width / 10; // Supposons une grille de 10x10
            int hauteurCase = panel.Height / 10;

            // Calculer la position en pixels
            int positionX = caseX * largeurCase;
            int positionY = caseY * hauteurCase;

            // Ajouter la position au dictionnaire
            AjouterPion(new Point(positionX, positionY), typePion);
        }

        public Dictionary<Point, string> ObtenirPositionsBleues()
        {
            return new Dictionary<Point, string>(PositionsPionsBleus);
        }

        public Dictionary<Point, string> ObtenirPositionsRouges()
        {
            return new Dictionary<Point, string>(PositionsPionsRouges);
        }
    }
}
