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
            Debug.WriteLine(joueur + "initialisation");
            AjouterEventsSurPieces();
        }
        public static void PositionnerTousLesPions(Panel panel, int largeurCase, int hauteurCase)
        {
            int taillePictureBoxX = (int)(largeurCase * 0.8);
            int taillePictureBoxY = (int)(hauteurCase * 0.8);

            foreach (Control ctrl in panel.Controls)
            {
                if (ctrl is PictureBox pb && pb.Tag is PieceInfo info)
                {
                    int col = info.PositionGrille.X;
                    int row = info.PositionGrille.Y;

                    pb.Size = new Size(taillePictureBoxX, taillePictureBoxY);
                    pb.Location = new Point(
                        (col - 1) * largeurCase + (largeurCase - taillePictureBoxX) / 2,
                        (row - 1) * hauteurCase + (hauteurCase - taillePictureBoxY) / 2
                    );
                }
            }
        }


        private void AjouterEventsSurPieces()
        {
            foreach (Control ctrl in panel.Controls)
            {
                if (ctrl is PictureBox pb && pb.Tag is PieceInfo info)
                {
                    bool isBluePiece = info.IsBlue;
                    bool estJoueurBleu = joueurActuel == Player.Player_Blue;

                    if (isBluePiece == estJoueurBleu)
                    {
                        pb.Click += Piece_Click;
                        Debug.WriteLine("dans le if");
                    }
                }
            }
        }

        private void Piece_Click(object sender, EventArgs e)
        {
            var clickedPiece = sender as PictureBox;
            if (clickedPiece == null) return;

            var clickedInfo = clickedPiece.Tag as PieceInfo;
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
                    selectedPiece.BackColor = clickedInfo.IsBlue ? Color.LightBlue : Color.LightCoral;
                    selectedPiece = null;
                    return;
                }

                var selectedInfo = (PieceInfo)selectedPiece.Tag;

                if (selectedInfo.IsBlue == clickedInfo.IsBlue)
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
                    selectedPiece.BackColor = selectedInfo.IsBlue ? Color.LightBlue : Color.LightCoral;
                    clickedPiece.BackColor = clickedInfo.IsBlue ? Color.LightBlue : Color.LightCoral;
                }
                else
                {
                    selectedPiece.BackColor = selectedInfo.IsBlue ? Color.LightBlue : Color.LightCoral;
                }

                selectedPiece = null;
            }
        }


        public void SupprimerEvents()
        {
            foreach (Control ctrl in panel.Controls)
            {
                if (ctrl is PictureBox pb && pb.Tag is PieceInfo info)
                {
                    bool isBluePiece = info.IsBlue;
                    bool estJoueurBleu = joueurActuel == Player.Player_Blue;

                    if (isBluePiece == estJoueurBleu)
                    {
                        Debug.WriteLine("dans le supprim");
                        pb.Click -= Piece_Click;
                    }
                }
            }
        }
    }
}
