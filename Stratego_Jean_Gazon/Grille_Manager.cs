using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stratego_Jean_Gazon
{
    public class Grille_Manager
    {
        private Panel PnlGrilleGame;
        private Panel pnlMenuPause;
        private PictureBox ptLac1;
        private PictureBox ptLac2;


        public Grille_Manager(Panel panel_Grille,Panel panel_Pause, PictureBox picture1, PictureBox picture2) {
            PnlGrilleGame = panel_Grille;
            pnlMenuPause = panel_Pause;
            ptLac1 = picture1;
            ptLac2 = picture2;
        }
        public void PnlGrilleGame_Dessine(object sender, PaintEventArgs e)
        {
            // Calculer la taille des cases
            int largeurCase = PnlGrilleGame.Width / 10;  // 10 cases en largeur
            int hauteurCase = PnlGrilleGame.Height / 10; // 10 cases en hauteur
            // Dessiner les lignes horizontales
            for (int i = 1; i < 10; i++) // Nous commençons à 1 car la première ligne est au-dessus de 0
            {
                e.Graphics.DrawLine(Pens.Black, 0, i * hauteurCase, PnlGrilleGame.Width, i * hauteurCase);
            }

            // Dessiner les lignes verticales
            for (int j = 1; j < 10; j++) // Idem pour les colonnes
            {
                e.Graphics.DrawLine(Pens.Black, j * largeurCase, 0, j * largeurCase, PnlGrilleGame.Height);
            }
            Place_ptlac(2, 4, largeurCase, hauteurCase, ptLac1);
            Place_ptlac(6, 4, largeurCase, hauteurCase, ptLac2);

        }
        private void Place_ptlac(int positionx, int positiony, int largeurcase, int hauteurcase, PictureBox picturebox)
        {
            int x = positionx * largeurcase; // Position X pour les colonnes 5 et 6
            int y = positiony * hauteurcase; // Position Y pour les lignes 3 et 4
            int width = 2 * largeurcase; // Largeur pour couvrir les colonnes 5 et 6
            int height = 2 * hauteurcase; // Hauteur pour couvrir les lignes 3 et 4
            picturebox.SetBounds(x, y, width, height);

        }
        public void CenterPanel(int ClientSizeW,int ClientSizeH)
        {
            pnlMenuPause.Left = (ClientSizeW - pnlMenuPause.Width) / 2;
            pnlMenuPause.Top = (ClientSizeH - pnlMenuPause.Height) / 2;
        }

        private void CreateStrategoPictureBoxes(int startX, int startY, bool isBlue)
        {
            string equipe = isBlue ? "bleu" : "rouge";
            var personnages = new (string nom, int count)[]
            {
        ("Maréchal", 1), ("Général", 1), ("Colonel", 1),
        ("Commandant", 1), ("Capitaine", 1), ("Lieutenant", 2),
        ("Sergent", 3), ("Démineur", 6), ("Adjudant", 2),
        ("Éclaireur", 4), ("Espion", 1), ("Bombe", 6),
        ("Drapeau", 1)
            };

            int largeurCase = PnlGrilleGame.Width / 10;
            int hauteurCase = PnlGrilleGame.Height / 10;
            int taillePictureBoxX = (int)(largeurCase * 0.8);
            int taillePictureBoxY = (int)(hauteurCase * 0.8);
            Color equipeColor = isBlue ? Color.LightBlue : Color.LightCoral;

            int xPosition = startX;
            int yPosition = startY;

            foreach (var (nom, count) in personnages)
            {
                for (int i = 0; i < count; i++)
                {
                    PictureBox pictureBox = new PictureBox
                    {
                        Name = $"{nom}_{equipe}{i + 1}",
                        Size = new Size(taillePictureBoxX, taillePictureBoxY),
                        BackColor = equipeColor
                    };

                    // Centrer la PictureBox dans la case
                    pictureBox.Location = new Point(
                        xPosition - (taillePictureBoxX / 2),
                        yPosition - (taillePictureBoxY / 2)
                    );
                    pictureBox.Tag = isBlue;

                    PnlGrilleGame.Controls.Add(pictureBox);

                    xPosition += largeurCase;
                    if (xPosition >= PnlGrilleGame.Width)
                    {
                        xPosition = startX;
                        yPosition += hauteurCase;
                    }
                }
            }
        }
        public void Piece_Init()
        {
            int largeurCase = PnlGrilleGame.Width / 10;
            int hauteurCase = PnlGrilleGame.Height / 10;

            // Calcul du point de départ pour la première pièce (équipe bleue)
            int Case1BasG_X = (largeurCase * 0) + (largeurCase / 2);
            int Case1BasG_Y = (hauteurCase * 7) + (hauteurCase / 2);

            // Calcul du point de départ pour la deuxième pièce (équipe rouge)
            int Case1hautG_X = (largeurCase * 0) + (largeurCase / 2);
            int Case1hautG_Y = (hauteurCase * 0) + (hauteurCase / 2);
            CreateStrategoPictureBoxes(Case1BasG_X, Case1BasG_Y, true);
            CreateStrategoPictureBoxes(Case1hautG_X, Case1hautG_Y, false);
        }

        public void Piece_Rezise(bool isblue)
        {
            int largeurCase = PnlGrilleGame.Width / 10;
            int hauteurCase = PnlGrilleGame.Height / 10;

            int taillePictureBoxX = (int)(largeurCase * 0.8);
            int taillePictureBoxY = (int)(hauteurCase * 0.8);

            int Case1BasG_X = (largeurCase * 0) + (largeurCase / 2);
            int Case1BasG_Y = (hauteurCase * 7) + (hauteurCase / 2);

            int Case1hautG_X = (largeurCase * 0) + (largeurCase / 2);
            int Case1hautG_Y = (hauteurCase * 0) + (hauteurCase / 2);
            Piece_Adaptation(Case1BasG_X, Case1BasG_Y, largeurCase, hauteurCase, taillePictureBoxX, taillePictureBoxY, isblue);

            Piece_Adaptation(Case1hautG_X, Case1hautG_Y, largeurCase, hauteurCase, taillePictureBoxX, taillePictureBoxY, !isblue);

        }

        private void Piece_Adaptation(int LocationX, int LocationY, int largeurCase, int hauteurCase, int taillePictureBoxX, int taillePictureBoxY, bool isBlueTeam)
        {
            int PositionX = LocationX;
            int PositionY = LocationY;

            int compteur = 0;

            foreach (Control PictureBox_piece in PnlGrilleGame.Controls)
            {
                if (PictureBox_piece is PictureBox pb && pb.Tag is bool pieceBlue && pieceBlue == isBlueTeam) // Filtrer selon la couleur
                {
                    compteur++;
                    ;



                    // Définir la taille correcte
                    pb.Size = new Size(taillePictureBoxX, taillePictureBoxY);

                    // Centrer la PictureBox dans la case
                    pb.Location = new Point(
                        PositionX - (taillePictureBoxX / 2),
                        PositionY - (taillePictureBoxY / 2)
                    );

                    // Déplacer vers la case suivante
                    PositionX += largeurCase;

                    // Si on dépasse la largeur du panel, passer à la ligne suivante
                    if (PositionX >= PnlGrilleGame.Width)
                    {
                        PositionX = LocationX;
                        PositionY += hauteurCase;
                    }
                }
            }
        }

        public void Player_Grille_Change(Player Current_Player)
        {
            if (Current_Player == Player.Player_Blue) 
            {
                Piece_Rezise(true);

            }
            else { Piece_Rezise(false); }

        }
    }
}
