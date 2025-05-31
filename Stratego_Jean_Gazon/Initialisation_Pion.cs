using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Stratego_Jean_Gazon
{
    public class Initialisation_Pion
    {
        private PictureBox selectedPiece = null;
        private Panel panel;
        private Player joueurActuel;

        public Initialisation_Pion(Panel grillePanel, Player joueur)
        {
            panel = grillePanel;
            joueurActuel = joueur;
            AjouterEventsSurPieces();
        }

        public static void PositionnerTousLesPions(Panel panel, int largeurCase, int hauteurCase)
        {
            int taillePictureBoxX = (int)(largeurCase * 0.8);
            int taillePictureBoxY = (int)(hauteurCase * 0.8);

            foreach (Control ctrl in panel.Controls)
            {
                if (ctrl is PictureBox pb && pb.Tag is personnage_base pion)
                {
                    int col = pion.PositionGrille.X;
                    int row = pion.PositionGrille.Y;

                    pb.Size = new Size(taillePictureBoxX, taillePictureBoxY);
                    pb.Location = new Point(
                        (col - 1) * largeurCase + (largeurCase - taillePictureBoxX) / 2,
                        (row - 1) * hauteurCase + (hauteurCase - taillePictureBoxY) / 2
                    );
                }
            }
        }

        private void AjouterEventsSurPieces() // fonction permetant de lier les événements de clic sur les pièces
        {
            foreach (Control ctrl in panel.Controls)
            {
                if (ctrl is PictureBox pb && pb.Tag is personnage_base pion)
                {
                    bool isBluePiece = pion.Couleur;  
                    bool estJoueurBleu = joueurActuel == Player.Player_Blue;// associe le joueur actuel à la couleur des pièces pour ne pas que le joueur d'enface puisse modifier les pièce de son adversaire 

                    if (isBluePiece == estJoueurBleu)
                    {
                        pb.Click += Piece_Click;
                    }
                }
            }
        }

        private void Piece_Click(object sender, EventArgs e) 
        {
            var clickedPiece = sender as PictureBox; // la pb est selectionnée 
            if (clickedPiece == null) return;

            var clickedInfo = clickedPiece.Tag as personnage_base; // on récupère les informations de la piece selectionnée
            if (clickedInfo == null) return;

            if (selectedPiece == null)
            {
                selectedPiece = clickedPiece;
                selectedPiece.BackColor = Color.Yellow;
            }
            else
            {
                if (selectedPiece == clickedPiece)
                {
                    selectedPiece.BackColor = clickedInfo.Couleur ? Color.LightBlue : Color.LightCoral;
                    selectedPiece = null;
                    return;
                }

                var selectedInfo = (personnage_base)selectedPiece.Tag;

                if (selectedInfo.Couleur == clickedInfo.Couleur)// on effectue le changerment de position 
                {
                    // Échange la position graphique
                    Point tmp = selectedPiece.Location;
                    selectedPiece.Location = clickedPiece.Location;
                    clickedPiece.Location = tmp;

                    // Échange la position logique
                    Point tmpPos = selectedInfo.PositionGrille;
                    selectedInfo.PositionGrille = clickedInfo.PositionGrille;
                    clickedInfo.PositionGrille = tmpPos;

                    // Remettre la couleur d'origine
                    selectedPiece.BackColor = selectedInfo.Couleur ? Color.LightBlue : Color.LightCoral;
                    clickedPiece.BackColor = clickedInfo.Couleur ? Color.LightBlue : Color.LightCoral;
                }
                else
                {
                    selectedPiece.BackColor = selectedInfo.Couleur ? Color.LightBlue : Color.LightCoral;
                }

                selectedPiece = null;
            }
        }

        public void SupprimerEvents()
        {
            foreach (Control ctrl in panel.Controls)
            {
                if (ctrl is PictureBox pb && pb.Tag is personnage_base pion)
                {
                    bool isBluePiece = pion.Couleur;
                    bool estJoueurBleu = joueurActuel == Player.Player_Blue;

                    if (isBluePiece == estJoueurBleu)
                    {
                        pb.Click -= Piece_Click;
                    }
                }
            }
        }
    }
}
